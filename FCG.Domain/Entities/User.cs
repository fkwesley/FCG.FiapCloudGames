using System.Text.RegularExpressions;

namespace FCG.FiapCloudGames.Core.Entities
{
    public class User
    {
        public required string UserId { get; set; }
        public required string Name { get; set; }
        private string _email { get; set; }         
        private string _password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsAdmin { get; set; } = false;

        // Propriedade pública com validação de e-mail no `set`.
        public string Email
        {
            get => _email;
            set
            {
                if (!EmailRegex.IsMatch(value)) // Se o valor não corresponde ao padrão de e-mail...
                    throw new ArgumentException("Invalid email format."); // ...lança exceção.
                _email = value; // Se válido, atribui ao campo privado.
            }
        }

        // Propriedade pública com validação de senha forte no `set`.
        public string Password
        {
            get => _password;
            set
            {
                if (!StrongPasswordRegex.IsMatch(value)) // Se não for uma senha segura...
                    throw new ArgumentException("Password must be at least 8 characters and include letters, numbers and special characters."); // ...lança exceção.
                _password = value; // Se válida, atribui ao campo privado.
            }
        }

        // Expressão regular para validar formato básico de e-mail.
        // Garante que tenha algo antes e depois de @ e depois um ponto.
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        // Expressão regular para validar senha forte:
        // - Pelo menos uma letra
        // - Pelo menos um número
        // - Pelo menos um caractere especial
        // - Mínimo de 8 caracteres
        private static readonly Regex StrongPasswordRegex =
            new(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", RegexOptions.Compiled);
    }
}
