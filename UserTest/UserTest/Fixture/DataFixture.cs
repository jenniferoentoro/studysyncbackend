// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Bogus;
// using user_service.model;

// namespace UserTest.Fixture
// {
//     public class DataFixture
//     {
//         public static List<User> GetUsers(int count, bool useNewSeed = false)
//         {
//             return GetUserFaker(useNewSeed).Generate(count);
//         }

//         public static User GetUser(bool useNewSeed = false)
//         {
//             return GetUsers(1, useNewSeed)[0];
//         }

//         public static Faker<User> GetUserFaker(bool useNewSeed = false)
//         {
//             var seed = 0;
//             if (useNewSeed)
//             {
//                 seed = Random.Shared.Next(10, int.MaxValue);
//             }

//             return new Faker<User>()
//                 .RuleFor(t => t.Id, o => "userid" + o.UniqueIndex)
//                 .RuleFor(t => t.Name, o => o.Name.FullName())
//                 .RuleFor(t => t.Email, o => o.Internet.Email())
//                 .RuleFor(t => t.School, o => o.Company.CompanyName())
//                 .RuleFor(t => t.Grade, o => user_service.Model.Grade.HighSchool).UseSeed(seed);

//         }
//     }
// }
