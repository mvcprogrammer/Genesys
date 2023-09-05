namespace GenesysCloud.QueryHandlers.Mock.DataGenerators;

public static class UserDataGenerator
{
    public static UserEntityListing GenerateUserEntityListing()
    {
        const string userListJson = @"[
        {
            ""Email"": ""6538435@mvcprogrammer.com"",
            ""Name"": ""James Smith"",
            ""Title"": ""Software Engineer"",
            ""Id"": ""6538435""
        },
        {
            ""Email"": ""9846289@mvcprogrammer.com"",
            ""Name"": ""Olivia Johnson"",
            ""Title"": ""Software Engineer"",
            ""Id"": ""9846289""
        },
        {
            ""Email"": ""9532459@mvcprogrammer.com"",
            ""Name"": ""William Brown"",
            ""Title"": ""Quality Assurance"",
            ""Id"": ""9532459""
        },
        {
            ""Email"": ""6513224@mvcprogrammer.com"",
            ""Name"": ""Emma Davis"",
            ""Title"": ""Software Engineer"",
            ""Id"": ""6513224""
        },
        {
            ""Email"": ""2648766@mvcprogrammer.com"",
            ""Name"": ""Liam Miller"",
            ""Title"": ""Quality Assurance"",
            ""Id"": ""2648766""
        },
        {
            ""Email"": ""2346847@mvcprogrammer.com"",
            ""Name"": ""Ava Wilson"",
            ""Title"": ""Product Management"",
            ""Id"": ""2346847""
        },
        {
            ""Email"": ""5762166@mvcprogrammer.com"",
            ""Name"": ""Benjamin Taylor"",
            ""Title"": ""Software Engineer"",
            ""Id"": ""5762166""
        },
        {
            ""Email"": ""5432167@mvcprogrammer.com"",
            ""Name"": ""Sophia Lee"",
            ""Title"": ""Product Management"",
            ""Id"": ""5432167""
        },
        {
            ""Email"": ""7634547@mvcprogrammer.com"",
            ""Name"": ""Michael Harris"",
            ""Title"": ""Software Engineer"",
            ""Id"": ""7634547""
        },
        {
            ""Email"": ""5865465@mvcprogrammer.com"",
            ""Name"": ""Isabella Clark"",
            ""Title"": ""Quality Assurance"",
            ""Id"": ""5865465""
        }
      ]";
        
        var userList = userListJson.JsonDeserializeFromString<List<User>>();
        
        return new UserEntityListing
        {
            Entities = userList,
            PageSize = 25,
            PageCount = 1,
            Total = userList.Count
        };
    }
}

