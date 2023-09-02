namespace GenesysCloud.QueryHandlers.Mock.DataGenerators;

public static class UserDataGenerator
{
    public static UserEntityListing GenerateUserData()
    {
        var users = new List<User>();
        var names = UserNames();
        var rng = new Random();

        foreach (var name in names)
        {
            var user = new User
            {
                Name = name,
                Division = new Division
                {
                    Name = GetRandomDivision(rng)
                },
                Chat = new Chat
                {
                    JabberId = ""
                }
            };

            users.Add(user);
        }

        return new UserEntityListing
        {
            Entities = users,
            PageCount = 1
        };
    }
    
    private static IEnumerable<string> UserNames()
    {
        return new[]
        {
            "James Smith",
            "Olivia Johnson",
            "William Brown",
            "Emma Davis",
            "Liam Miller",
            "Ava Wilson",
            "Benjamin Taylor",
            "Sophia Lee",
            "Michael Harris",
            "Isabella Clark"
        };
    }

    private static string[] Divisions()
    {
        return new[] 
        {
            "Software Development",
            "Quality Assurance",
            "Product Management"
        };
    }

    private static string GetRandomDivision(Random rng)
    {
        var divisions = Divisions();
        var roll = rng.Next(100);

        return roll switch
        {
            < 80 => divisions[0],
            < 90 => divisions[1],
            _ => divisions[2]
        };
    }
}

