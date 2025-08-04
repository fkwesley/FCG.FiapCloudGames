import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    vus: 5,           // 10 usuários simultâneos
    iterations: 500,   // Total de 100 requisições
};

// 🔐 Autenticação única
export function setup() {
    const loginPayload = JSON.stringify({
        userId: 'CJ',
        password: 'Password1*',
    });

    const loginHeaders = {
        'accept': 'text/plain',
        'Content-Type': 'application/json',
    };

    const loginRes = http.post(
        'https://aca-fcg-uat.agreeablemushroom-99bd6ac3.brazilsouth.azurecontainerapps.io/Auth/Login',
        loginPayload,
        { headers: loginHeaders }
    );

    check(loginRes, {
        'Login retornou 200': (res) => res.status === 200,
    });

    // ✅ Aqui está a correção importante:
    let token = '';
    try {
        const json = JSON.parse(loginRes.body);
        token = json.token;
        console.log("Token obtido:", token);
    } catch (err) {
        console.error("Erro ao parsear o token:", err, loginRes.body);
    }

    return { token };
}

export default function (data) {
    const userHeaders = {
        'accept': 'text/plain',
        'Authorization': `Bearer ${data.token}`,
    };

    const userRes = http.get(
        'https://aca-fcg-uat.agreeablemushroom-99bd6ac3.brazilsouth.azurecontainerapps.io/Games',
        { headers: userHeaders }
    );

    const ok = check(userRes, {
        'GET /Users retornou 200': (res) => res.status === 200,
    });

    if (!ok) {
        console.warn("Falha no GET /Games:", userRes.status, userRes.body);
    }

    sleep(0.2); // aguarda 200ms
}
