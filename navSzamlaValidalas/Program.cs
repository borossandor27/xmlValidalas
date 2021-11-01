using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace navSzamlaValidalas
{
    class Program
    {
        static string szamlakHelye = "PeldaSzamlak";
        static XmlReaderSettings invoices = new XmlReaderSettings();
        static void Main()
        {
            string semaPath = "semak";
            invoices.Schemas.Add("http://schemas.nav.gov.hu/NTCA/1.0/common", Path.Combine(semaPath, "common.xsd"));
            invoices.Schemas.Add("http://schemas.nav.gov.hu/OSA/3.0/base", Path.Combine(semaPath, "invoiceBase.xsd"));
            invoices.Schemas.Add("http://schemas.nav.gov.hu/OSA/3.0/annul", Path.Combine(semaPath, "invoiceAnnulment.xsd"));
            invoices.Schemas.Add("http://schemas.nav.gov.hu/OSA/3.0/api", Path.Combine(semaPath, "invoiceApi.xsd"));
            invoices.Schemas.Add("http://schemas.nav.gov.hu/OSA/3.0/data", Path.Combine(semaPath, "invoiceData.xsd"));
            invoices.Schemas.Add("http://schemas.nav.gov.hu/OSA/3.0/metrics", Path.Combine(semaPath, "serviceMetrics.xsd"));
            invoices.ValidationType = ValidationType.Schema;
            invoices.ValidationEventHandler += new ValidationEventHandler(invoicesSettingsValidationEventHandler);

            XmlReader invoice;

            foreach (string item in Directory.GetFiles(Path.Combine(szamlakHelye)))
            {
                invoice = XmlReader.Create(item, invoices);
                Console.WriteLine(item);
                try
                {
                    while (invoice.Read()) { }
                }
                catch (XmlException e)
                {

                    Console.WriteLine($"{e.SourceUri}: => {e.Message}");
                }
            }

            Console.WriteLine(Environment.NewLine + "Program vége!");
            Console.ReadLine();
        }

        static void invoicesSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.Write("WARNING: ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("ERROR: ");
                Console.WriteLine(e.Message);
            }
        }
    }
}

