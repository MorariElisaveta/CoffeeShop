using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RiseCup.Database;
using RiseCup.Domain.Models;

namespace RiseCup.BL.Services
{
    /// <summary>
    /// Сервис для управления пользователями, включает аутентификацию и регистрацию.
    /// </summary>
    public class UserManagementService
    {
        private readonly RiseCupContext _db;

        public UserManagementService(RiseCupContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Проверяет, правильны ли логин и пароль, и возвращает пользователя, если они верны.
        /// </summary>
        /// <param name="username">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Объект пользователя, если аутентификация успешна; иначе возвращает null.</returns>
        public User AuthenticateUser(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user != null && VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return user;
            }
            return null;
        }

        /// <summary>
        /// Регистрирует нового пользователя с заданными логином и паролем.
        /// </summary>
        /// <param name="username">Логин нового пользователя.</param>
        /// <param name="password">Пароль нового пользователя.</param>
        /// <returns>Новый объект пользователя, если регистрация успешна; иначе возвращает null.</returns>
        public User RegisterUser(string username, string password)
        {
            try
            {
                if (_db.Users.Any(u => u.Username == username))
                    return null; // Пользователь уже существует

                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                var newUser = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Username = username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = "User"  // Назначение роли по умолчанию
                };

                _db.Users.Add(newUser);
                _db.SaveChanges();

                return newUser;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Получаем ошибки валидации и превращаем их в строку для логирования или отладки
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /// <summary>
        /// Проверяет, соответствует ли роль пользователя требуемой роли.
        /// </summary>
        /// <param name="user">Пользователь для проверки.</param>
        /// <param name="requiredRole">Требуемая роль.</param>
        /// <returns>True, если пользователь имеет требуемую роль; иначе false.</returns>
        public bool IsUserAuthorized(User user, string requiredRole)
        {
            return user.Role == requiredRole;
        }

        /// <summary>
        /// Создает хеш пароля и соль, используя криптографический алгоритм HMACSHA512.
        /// </summary>
        /// <param name="password">Пароль, который нужно зашифровать.</param>
        /// <param name="passwordHash">Выходной параметр, содержащий хеш пароля.</param>
        /// <param name="passwordSalt">Выходной параметр, содержащий криптографическую соль, использованную при хешировании пароля.</param>
        /// <remarks>
        /// Соль - это уникальные данные, которые добавляются к паролю перед его хешированием.
        /// Это увеличивает безопасность хешированных паролей, предотвращая атаки по словарю и использование радужных таблиц.
        /// </remarks>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Проверяет, совпадает ли хеш введенного пароля с сохраненным хешем пароля.
        /// </summary>
        /// <param name="password">Пароль для проверки.</param>
        /// <param name="storedHash">Хранимый хеш пароля.</param>
        /// <param name="storedSalt">Хранимая соль, использованная при создании хеша.</param>
        /// <returns>Возвращает true, если хеши совпадают, иначе false.</returns>
        /// <remarks>
        /// Для проверки используется та же соль, которая была использована при создании хеша.
        /// Это обеспечивает сравнение актуального хеша введенного пароля с хранимым хешем.
        /// </remarks>
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
    }
}