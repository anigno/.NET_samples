#region

// Add Usings:
using System;
using System.Collections.Generic;
using System.Linq;
using OwinHost;

#endregion

namespace OwinClient
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            Console.WriteLine("Read all the companies...");
            CompanyClient companyClient = new CompanyClient("http://localhost:8080");
            IEnumerable<Company> companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);
            int nextId = (from c in companies select c.Id).Max() + 1;
            Console.WriteLine("Add a new company...");
            var result = companyClient.AddCompany(
                new Company
                {
                    Id = nextId,
                    Name = string.Format("New Company #{0}", nextId)
                });
            WriteStatusCodeResult(result);
            Console.WriteLine("Updated List after Add:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);
            Console.WriteLine("Update a company...");
            var updateMe = companyClient.GetCompany(nextId);
            updateMe.Name = string.Format("Updated company #{0}", updateMe.Id);
            result = companyClient.UpdateCompany(updateMe);
            WriteStatusCodeResult(result);
            Console.WriteLine("Updated List after Update:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);
            Console.WriteLine("Delete a company...");
            result = companyClient.DeleteCompany(nextId - 1);
            WriteStatusCodeResult(result);
            Console.WriteLine("Updated List after Delete:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);
            Console.Read();
        }

        private static void WriteCompaniesList(IEnumerable<Company> companies)
        {
            foreach (var company in companies)
            {
                Console.WriteLine("Id: {0} Name: {1}", company.Id, company.Name);
            }
            Console.WriteLine("");
        }

        private static void WriteStatusCodeResult(System.Net.HttpStatusCode statusCode)
        {
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Opreation Succeeded ‐ status code {0}", statusCode);
            }

            else
            {
                Console.WriteLine("Opreation Failed ‐ status code {0}", statusCode);
            }
            Console.WriteLine("");
        }

        #endregion
    }
}