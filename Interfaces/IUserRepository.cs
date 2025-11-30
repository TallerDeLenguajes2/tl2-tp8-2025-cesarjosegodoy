using System.Security.Cryptography.X509Certificates;
using SistemaVentas.Web.Models;

public interface IUserRepository
{
    // Retorna el objeto Usuario si las credenciales son válidas, sino null.
    Usuario GetUser(string username, string password);
}

