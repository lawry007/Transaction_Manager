using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Trans_Charge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Health Station Payment Gateway");
            Console.WriteLine("------------------");
            Console.WriteLine("Please enter the amount you want to pay below");
            Console.WriteLine("|||||||||||||||||||");
            Console.WriteLine("vvvvvvvvvvvvvvvvvvv");

            double Amount = double.Parse(Console.ReadLine());

            //checks if the amount entered is greater than 0
            if (Amount > 0)
            {
                if ((Amount > 50000) && (Amount <= 999999999))
                    calculateCharge(Amount, 2);
                else if ((Amount > 5000) && (Amount <= 50000))
                    calculateCharge(Amount, 1);
                else if ((Amount >= 1) && (Amount <= 5000))
                    calculateCharge(Amount, 0);
            }
            else if (Amount == 0)
                Console.WriteLine("The amount to be paid must be greater than 0.");
            else
                Console.WriteLine("The entered is invalid. Enter a valid amount.");

            
            Console.WriteLine("|||||||||||||||||||");
            Console.WriteLine("vvvvvvvvvvvvvvvvvvv");
            Console.WriteLine("Thanks for shopping with us.");
            Console.ReadKey();

        }

        // config file used to initiate the fee.confog.json file at startup
        public static dynamic configfile()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("fees.config.json");

            var config = builder.Build();
            var appConfig = config.GetSection("fees").Get<List<FeesConfig>>();

            return appConfig;

        }

        //method that calclates the actual amount to be paid by less of the transation charge 
        public static void calculateCharge(double Amount, int pos)
        {
            var fees = configfile();

            //checks for negative amount to be paid
            if (Amount < 10)
            {
                Console.WriteLine("The amount to be paid after discount is: 0 ");
                Console.WriteLine("The actual charge to be paid is: " + double.Parse(fees[pos].feeAmount));
            }
            else
            {
                var finalAmount = Amount - double.Parse(fees[pos].feeAmount);

                if (confirmCharge(finalAmount, pos) == false)
                {
                    Console.WriteLine("The amount to be paid after discount is: " + finalAmount);
                    Console.WriteLine("The actual charge to be paid is: " + double.Parse(fees[pos].feeAmount)); 
                }
                else
                {
                    Console.WriteLine("The amount to be paid after discount is: " + finalAmount);
                    Console.WriteLine("After discount, the charge has changed, the actual charge to be paid is: " + double.Parse(fees[pos].feeAmount));
                }
            }


        }

        //this method is used to compare the transaction charge before and after subtraction 

        public static bool confirmCharge(double Amount, int oldFeeSection)
        {
            bool hasChanged = false;
            int newFeeSection;

            if ((Amount > 50000) && (Amount <= 999999999))
                newFeeSection = 2;
            else if ((Amount > 5000) && (Amount <= 50000))
                newFeeSection = 1;
            else
                newFeeSection = 0;

            if (oldFeeSection == newFeeSection)
                hasChanged = false;
            else
                hasChanged = true;

            return hasChanged;


        }
    }
}
