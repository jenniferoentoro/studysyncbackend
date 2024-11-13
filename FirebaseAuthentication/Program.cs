using System;
using Firebase.Auth;
using System.Threading.Tasks;
using Refit;
namespace FirebaseAuthentication;

class Program
{
    private const string API_KEY = "AIzaSyBMZ3zH93v8lXaXh4Lqd6GnWw_qBlq9T0c";
    static async Task Main(string[] args)
    {
        FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(API_KEY));

        FirebaseAuthLink firebaseAuthLink = await firebaseAuthProvider.SignInWithEmailAndPasswordAsync("jenniferoentoro@gmail.com", "jenjen123");

        Console.WriteLine(firebaseAuthLink.FirebaseToken);
        IDataService dataService = RestService.For<IDataService>("https://localhost:5000");

        await dataService.GetData(firebaseAuthLink.FirebaseToken);
    }
}

public interface IDataService
{
    [Get("/")]
    Task<string> GetData([Authorize("Bearer")] string token);
}