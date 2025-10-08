import http from 'k6/http';
http.setResponseCallback(http.expectedStatuses({ min: 0, max: 599 }));

import { check, sleep } from 'k6';
import { Counter } from 'k6/metrics';

const BASE_URL = 'https://aca-fcg.agreeablemushroom-99bd6ac3.brazilsouth.azurecontainerapps.io';
//const BASE_URL = 'https://localhost:7243';

// Métricas
export const post201 = new Counter('post_201');
export const post400 = new Counter('post_400');
export const post403 = new Counter('post_403');

export const get200 = new Counter('get_200');
export const get404 = new Counter('get_404');
export const get403 = new Counter('get_403');

export const put200 = new Counter('put_200');
export const put403 = new Counter('put_403');
export const put400 = new Counter('put_400');
export const put404 = new Counter('put_404'

export const delete204 = new Counter('delete_204');
export const delete403 = new Counter('delete_403');
export const delete404 = new Counter('delete_404');

const users = [
    { userId: 'CJ', password: 'Password1*' },
    { userId: 'max.payne', password: 'teste123*' },
    { userId: 'frank.vieira', password: 'Password1*' }
];

export let options = {
    //vus: 10, // Número de usuários virtuais
    //duration: '15s', // Duração do teste
    stages: [
        { duration: '3m', target: 60 },
        { duration: '5m', target: 120 },
        { duration: '2m', target: 40 }
    ]
};

export function setup() {
    const tokens = {};

    for (const user of users) {
        const res = http.post(`${BASE_URL}/Auth/Login`, JSON.stringify(user), {
            headers: { 'Content-Type': 'application/json' }
        });

        check(res, { [`Login ${user.userId} (200)`]: (r) => r.status === 200 });

        try {
            tokens[user.userId] = JSON.parse(res.body).token;
        } catch (err) {
            console.error(`Erro ao parsear token de ${user.userId}:`, err, res.body);
        }
    }

    return { tokens };
}

export default function (data) {
    const user = users[Math.floor(Math.random() * users.length)];
    const token = data.tokens[user.userId];

    const headers = {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json',
    };

    const isAdmin = user.userId === 'frank.vieira';
    const gameId = __ITER;
    const randomId = Math.floor(Math.random() * 11000) + 1;
    const gameName = `GTA ${randomId}`;

    // 1. POST
    const postPayload = JSON.stringify({
        name: gameName,
        description: 'Grand Theft Auto',
        genre: 'Action',
    });

    const postRes = http.post(`${BASE_URL}/Games`, postPayload, { headers });

    if (isAdmin) {
        check(postRes, {'POST /Games (201 ou 400)': (r) => r.status === 201 || r.status === 400});

        if (postRes.status === 201) {
            post201.add(1);
        } else if (postRes.status === 400) {
            post400.add(1);
        } else {
            console.error(`POST (admin) erro inesperado: ${postRes.status}, body: ${postRes.body}`);
        }
    } else {
        check(postRes, { 'POST /Games NotAdmin (403)': (r) => r.status === 403 });

        if (postRes.status === 403) {
            post403.add(1);
        } else {
            console.error(`POST (não-admin) erro: ${postRes.status}, body: ${postRes.body}`);
        }
    }

    // 2. GET /Games/{id}
    const getRes = http.get(`${BASE_URL}/Games/${gameId}`, { headers });

    if (getRes.status === 200) get200.add(1);
    else if (getRes.status === 403) get403.add(1);
    else if (getRes.status === 404) get404.add(1);
    else console.error(`GET /Games/${gameId} erro: ${getRes.status}, body: ${getRes.body}`);

    // 3. PUT ou DELETE aleatório
    const action = Math.random() < 0.5 ? 'PUT' : 'DELETE';

    if (action === 'PUT') {
        const putPayload = JSON.stringify({
            name: `Updated ${gameName}`,
            description: 'Atualizado',
            genre: 'Updated genre',
            rating: 5,
            releaseDate: new Date().toISOString().split('T')[0]
        });

        const putRes = http.put(`${BASE_URL}/Games/${gameId}`, putPayload, { headers });

        if (isAdmin) {
            check(putRes, { 'PUT /Games (200, 400 ou 404)': (r) => r.status === 200 || r.status === 404 || r.status === 400 });
            if (putRes.status === 200) put200.add(1);
            else if (putRes.status === 400) put400.add(1);
            else if (putRes.status === 404) put404.add(1);
            else console.error(`PUT erro: ${putRes.status}, body: ${putRes.body}`);
        } else {
            check(putRes, { 'PUT /Games NotAdmin (403)': (r) => r.status === 403 });
            if (putRes.status === 403) put403.add(1);
            else console.error(`PUT (não-admin) erro: ${putRes.status}, body: ${putRes.body}`);
        }

    } else {
        const delRes = http.del(`${BASE_URL}/Games/${gameId}`, null, { headers });

        if (isAdmin) {
            check(delRes, { 'DELETE /Games (204 ou 404)': (r) => r.status === 204 || r.status === 404 });
            if (delRes.status === 204) delete204.add(1);
            else if (delRes.status === 404) delete404.add(1);
            else console.error(`DELETE erro: ${delRes.status}, body: ${delRes.body}`);
        } else {
            check(delRes, { 'DELETE /Games NotAdmin (403)': (r) => r.status === 403 });
            if (delRes.status === 403) delete403.add(1);
            else console.error(`DELETE (não-admin) erro: ${delRes.status}, body: ${delRes.body}`);
        }
    }

    sleep(0.3);
}
