#MemFlex

MemFlex is a look at what is possible in an ASP.NET MVC application if you eschew the ASP.NET providers for membership and roles and build something from scratch with some simple requirements:

- Support all the actions of the MVC 4 Internet AccountController (register, login, login with OAuth). 

- Be test friendly (by providing interface definitions for both clients and dependencies)

- Run without ASP.NET (for integration tests and database migrations, as two examples). 

- Work with a variety of data sources (two common scenarios for storing use accounts these days involve document databases and web services).

Here are parts of the project as they exist today. 

##Sample Application

The sample application is a typical MVC 4 Internet application where the AccountController and database migrations use the FlexMembershipProvider. There are classes provided for working with both the Entity Framework and RavenDB, and you can (almost) switch between the two by adjusting an assembly reference and the namespaces you use.

After you’ve defined what a User model should look like, the first part would be configuring a FlexMembershipUserStore to work with your custom user type.
    
    using FlexProviders.EF; 
    
    namespace LogMeIn.Models 
    {    
        public class UserStore : FlexMembershipUserStore<User>    
        {        
            public UserStore(MovieDb db) : base(db)        
            {                    
            }    
         }
    }
    
The above code snippet, which uses EF, just requires a generic type parameter (your user type), and a DbContext object to work with. The UserStore is then plugged into the FlexMembershipProvider. You can do this by hand, or let an IoC container take care of the work. 

    var membership = new FlexMembershipProvider(    
        new UserStore(context),     
        new AspnetEnvironment()
    );

Once the FlexMembershipProvider is initialized inside a controller, you can use an API that looks a bit like the traditional ASP.NET Membership API. 

    _membershipProvider.Login(model.UserName, model.Password

##Everything Else
The FlexProviders part of the project consists of 4 pieces: the integrations tests, a RavenDB user store, an EF user store, and the FlexProviders themselves. 

###FlexProviders
The FlexProviders project defines the basic abstractions for a flexible membership system. For example, the interface definition to work with locally registered users (for now, a separate interface provides the OAuth functionality):

    public interface IFlexMembershipProvider
    {            
        bool Login(string username, string password);    
        void Logout();    
        void CreateAccount(IFlexMembershipUser user);    
        bool HasLocalAccount(string username);                    
        bool ChangePassword(string username, string oldPassword, string newPassword);
    }

There is also a concrete implementation of a flexible membership provider:

    public class FlexMembershipProvider : IFlexMembershipProvider,
                                          IFlexOAuthProvider,
                                          IOpenAuthDataProvider 
    {    
        public FlexMembershipProvider(IFlexUserStore userStore,         
                                      IApplicationEnvironment applicationEnvironment)                
        {
            _userStore = userStore;
            _applicationEnvironment = applicationEnvironment;    
        }    
        
        public bool Login(string username, string password)    
        {        
            var user = _userStore.GetUserByUsername(username);
            if(user == null)        {            
                return false;        
            }
            
            // ... omitted for brevity ...    
        }    
            
            // ... 
      }

Of course since the membership provider requires an IFlexUserStore dependency, the operations required for data access are defined in this project in the IFlexUserStore interface. There is also an AspnetEnvironment class that removes hard dependencies on test unfriendly bits like HttpContext.Current.

    public class AspnetEnvironment : IApplicationEnvironment
    {    
        public void IssueAuthTicket(string username, bool persist)    
        {        
            FormsAuthentication.SetAuthCookie(username,persist);                
        }    
        
        // ...
    }

###FlexProviders.EF and FlexProviders.Raven
It’s relatively straightforward to build classes that will take care of the data access required by a membership provider. For both Raven and EF, all you really need is a generic type parameter and a unit of work. For Raven:

    namespace FlexProviders.Raven
    {    
        public class FlexMembershipUserStore<TUser>         
            : IFlexUserStore where TUser : class, new()    
        {        
            private readonly IDocumentSession _session;
            public FlexMembershipUserStore(IDocumentSession session)
            {
                _session = session;        
            }        
            
            public IFlexMembershipUser GetUserByUsername(string username)        
            {
                return _session.Query<TUser>().SingleOrDefault(u => u.Username == username);        
            } 
            
            // ...    
            }
    }

And the EF version:

    namespace FlexProviders.EF
    {    
        public class FlexMembershipUserStore<TUser>
            : IFlexUserStore where TUser: class, IFlexMembershipUser, new()             
        { 
            private readonly DbContext _context;
            
            public FlexMembershipUserStore (DbContext context)        
            {
                _context = context;
            }
            
            public IFlexMembershipUser GetUserByUsername(string username)
            {
                return _context.Set<TUser>().SingleOrDefault(u => u.Username == username);   
            } 
            
            // ...
        }
      }

###FlexProviders.Tests
This project is a set of integration tests to verify the EF and Raven providers actually put and retrieve data with real databases. The EF tests require the DTC to be running (net start msdtc). The tests are configured to use SQL 2012 LocalDB (for EF) by default, while the Raven tests use Raven’s in-memory embedded mode. 
