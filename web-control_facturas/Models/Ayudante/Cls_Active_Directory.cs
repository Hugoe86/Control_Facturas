using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Configuration;

namespace web_cambios_procesos.Models.Ayudante
{
    public class Cls_Active_Directory
    {
        public class Users
        {
            public string EmailAddress { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public bool isMapped { get; set; }

            public string AccountName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Initials { get; set; }
        }
        public List<Users> GetADUsers()
        {
            List<Users> lstADUsers = new List<Users>();
            try
            {
                string DomainPath = "LDAP://DC=xxxx,DC=com";
                DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
                DirectorySearcher search = new DirectorySearcher(searchRoot);
                search.Filter = "(&(objectClass=user)(objectCategory=person))";
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("usergroup");
                search.PropertiesToLoad.Add("displayname");//first name
                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        string UserNameEmailString = string.Empty;
                        result = resultCol[counter];
                        if (result.Properties.Contains("samaccountname") &&
                                 result.Properties.Contains("mail") &&
                            result.Properties.Contains("displayname"))
                        {
                            Users objSurveyUsers = new Users();
                            objSurveyUsers.EmailAddress = (String)result.Properties["mail"][0] +
                              "^" + (String)result.Properties["displayname"][0];
                            objSurveyUsers.UserName = (String)result.Properties["samaccountname"][0];
                            objSurveyUsers.DisplayName = (String)result.Properties["displayname"][0];
                            lstADUsers.Add(objSurveyUsers);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el método (GetADUsers) de la clase (Cls_Active_Directory). Descripción: " + ex.Message);
            }
            return lstADUsers;
        }
        public List<Users> SearchActiveDirectory(string UserName)
        {
            List<Users> users = new List<Users>();
            try
            {
                //UserName = 'Aqui va tu Usuario';
                using (DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["ActiveDirectoryPath"]))
                {
                    using (DirectorySearcher Searcher = new DirectorySearcher(entry))
                    {
                        Searcher.SearchScope = SearchScope.Subtree;
                        Searcher.Filter = string.Format("(&(ObjectClass=User)(|(DisplayName={0}*)))", UserName);

                        Searcher.PropertiesToLoad.Add("DisplayName");
                        Searcher.PropertiesToLoad.Add("samAccountName");
                        Searcher.PropertiesToLoad.Add("givenName"); //FirstName in IPF
                        Searcher.PropertiesToLoad.Add("sn"); //LastName in IPF
                        Searcher.PropertiesToLoad.Add("initials");
                        Searcher.PropertiesToLoad.Add("mail"); //emailAddress in IPF
                        Searcher.SizeLimit = 100;

                        using (SearchResultCollection Results = Searcher.FindAll())
                        {
                            foreach (SearchResult result in Results)
                            {
                                Users item = new Users();
                                item.DisplayName = result.Properties.Contains("DisplayName")
                                     ? result.Properties["DisplayName"][0].ToString()
                                     : string.Empty;
                                item.AccountName = result.Properties["samAccountName"][0].ToString();
                                item.FirstName = result.Properties.Contains("givenName")
                                    ? result.Properties["givenName"][0].ToString()
                                    : string.Empty;
                                item.LastName = result.Properties.Contains("sn")
                                    ? result.Properties["sn"][0].ToString()
                                    : string.Empty;
                                item.Initials = result.Properties.Contains("initials")
                                    ? result.Properties["initials"][0].ToString()
                                    : string.Empty;
                                item.EmailAddress = result.Properties.Contains("mail")
                                    ? result.Properties["mail"][0].ToString()
                                    : string.Empty;
                                users.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el método (SearchActiveDirectory) de la clase (Cls_Active_Directory). Descripción: " + ex.Message);
            }
            return users;
        }
    }
}