using Simple.Data;

namespace FlexProviders.Tests.Integration
{
    public static class TestDB
    {
         public static dynamic Open()
         {
             return Database.OpenFile("TestDB.sdf");
         }
    }
}