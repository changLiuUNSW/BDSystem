using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using DataAccess.Console.Migration.Excel;
using DataAccess.Console.Report;
using DataAccess.Console.Scripts;
using DataAccess.Console.Scripts.Serialization;
using DataAccess.Console.Scripts.Types;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.DbContexts;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Serializers;
using DateAccess.Services.ContactService.Call.Scripts.Visitors;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.TravelPattern;
using DateAccess.Services.Excel;
using Copier = DataAccess.Console.Migration.DB.Copier;

namespace DataAccess.Console
{
    enum Options
    {
        DB = 1,
        EXCEL = 2,
        SCRIPT = 3,
        Report = 4,
        PhoneBook = 5,
        Test = 11
    }

    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu()
        {
            var menu = new StringBuilder("1. Copy database \n2. Copy excel\n3. Copy xml script\n4. Report\n5. PhoneBook");
            System.Console.WriteLine(menu);

            var input = System.Console.ReadLine();
            int option;
            if (!int.TryParse(input, out option))
            {
                System.Console.WriteLine("Invalid option");
            }
            else
            {
                switch (option)
                {
                    case (int)Options.DB:
                        Copier.Begin();
                        break;
                    case (int)Options.EXCEL:
                        var excels = new List<ExcelCopier>
                        {
                            new WorkbookCopier(@"K:\SQL_DATA\BD\Estimation Pipeline 2014 06 25.xlsm"),
                            //new QualificationCopier(@"C:\Users\jing\Desktop\Doc\BD database verticals 2015 02 24.rock.xlsx"),
                            //new SmallQuoteCopier(@"K:\SQL_DATA\BD\estimation\Small-Medium quote costing workbook_v8.30.test.xlsm"),
                            //new SpecCopier(@"K:\SQL_DATA\BD\estimation\Small-Medium quote costing workbook_v8.30.test.xlsm")
                        };

                        foreach (var excel in excels)
                        {
                            excel.Begin();
                        }

                        break;
                    case (int)Options.SCRIPT:
                        var file = AppDomain.CurrentDomain.BaseDirectory + "script.xml";
                        var template = new ScriptXmlTemplate();
                        var factory = new ScriptFactory();
                        var qualifications = new QualificationCreator();
                        var cleaningQuestions = new CleaningQuestionCreator();
                        foreach (var script in factory)
                        {
                            template.Scripts.Add(script.Create());
                        }
                        foreach (var creator in qualifications.Creators)
                        {
                            template.Scripts.Add(creator.Create());
                        }
                        foreach (var creator in cleaningQuestions.Creators)
                        {
                            template.Scripts.Add(creator.Create());
                        }

                        SerializeHelper.Create(file, template); 
                        System.Console.WriteLine("File is saved to " + file);
                        System.Console.ReadLine();
                        break;
                    case (int)Options.Report:

                        using (var context = new SiteResourceEntities())
                        {
                            var helper = new ReportHelper(new UnitOfWork(context));

                            foreach (var report in context.WeeklyReports)
                            {
                                context.Entry(report).State = EntityState.Deleted;
                            }

                            helper.GenerateWeeklyHistory(DateTime.Today);
                            helper.GenerateWeeklyHistory(DateTime.Today.AddDays(-7));
                            helper.GenerateWeeklyHistory(DateTime.Today.AddDays(-14));


                            foreach (var report in context.FullReports)
                            {
                                context.Entry(report).State = EntityState.Deleted;
                            }

                            helper.GenerateFullHistory(DateTime.Today);
                            helper.GenerateFullHistory(DateTime.Today.AddDays(-7));
                            helper.GenerateFullHistory(DateTime.Today.AddDays(-14));
                            context.SaveChanges();
                        }

                        break;
                    case (int)Options.PhoneBook:
                        System.Console.WriteLine("Please enter the Excel path");
                        var filePath = System.Console.ReadLine();
                        if (!System.IO.File.Exists(filePath))
                        {
                            System.Console.WriteLine("Error: File does not exists at {0}", filePath);
                            System.Console.ReadLine();
                        }
                        else
                        {
                            new PhoneBookCopier(filePath).Begin();
                        }
                        break;
                    case (int)Options.Test:
                        new SpecCopier(@"K:\SQL_DATA\BD\estimation\Small-Medium quote costing workbook_v8.30.test.xlsm").Begin();
                        break;
                    default:
                        System.Console.WriteLine("Invalid option");
                        System.Console.ReadLine();
                        break;
                }
            }
        }
    }
}

