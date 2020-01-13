using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace Diplom
{
    class ClientOperations
    {
        public void ShowMyAccount(ClientAllData clientAllData)
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"\n{clientAllData.Intro()}\n");
            Console.WriteLine(new string('-', 50));
        }

        public void BalanceOperation(ClientAllData clientAllData)
        {
            string balance = "Balance";
            Console.WriteLine("1. Начислить средства\n2. Снять средства");
            string balanceInput = Console.ReadLine();

            switch (balanceInput)
            {
                case "1":
                    WriteNewValueInDB(balance, PutMoney(clientAllData), clientAllData);
                    break;
                case "2":
                    WriteNewValueInDB(balance, WithdrawMoney(clientAllData), clientAllData);
                    break;
                default:
                    Console.WriteLine("Повторите ввод");
                    break;
            }
        }

        public void CreditOperation(ClientAllData clientAllData)
        {
            string credit = "Credit";
            Console.WriteLine("1. Погасить кредит\n2. Взять кредит");
            string creditInput = Console.ReadLine();

            switch (creditInput)
            {
                case "1":
                    WriteNewValueInDB(credit, PayCreditMoney(clientAllData), clientAllData);
                    break;
                case "2":
                    WriteNewValueInDB(credit, GetCredit(clientAllData), clientAllData);
                    break;
                default:
                    Console.WriteLine("Повторите ввод");
                    break;
            }
        }

        public void DepositOperation(ClientAllData clientAllData)
        {
            string deposit = "Deposit";
            Console.WriteLine("1. Положить деньги на депозит\n2. Снять деньги с депозита");
            string depositInput = Console.ReadLine();

            switch (depositInput)
            {
                case "1":
                    WriteNewValueInDB(deposit, PutDeposit(clientAllData), clientAllData);
                    break;
                case "2":
                    WriteNewValueInDB(deposit, WithdrowDeposit(clientAllData), clientAllData);
                    break;
                default:
                    Console.WriteLine("Повторите ввод");
                    break;
            }
        }

        private void WriteNewValueInDB(string columnName, decimal sum, ClientAllData clientAllData)
        {
            DBConnector connector = new DBConnector();

            string connStr = connector.GetConnectionString("Persons");
            string query = null;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                query = $"update ClientInfo set {columnName} = {sum} where ID = {clientAllData.InfoID}";

                SqlCommand command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        private decimal PutMoney(ClientAllData clientAllData)
        {
            while (true)
            {
                Console.Write("Сумма для зачисления - ");
                var inputAmount = Console.ReadLine();

                decimal operationSum = 0;

                IncorrectInput(inputAmount, out operationSum);

                clientAllData.Balance += operationSum;
                Console.WriteLine($"На счет внесено {operationSum}, сумма на счету {clientAllData.Balance}");
                break;
            }

            return clientAllData.Balance;
        }

        private decimal WithdrawMoney(ClientAllData clientAllData)
        {
            while (true)
            {
                if(clientAllData.Balance == 0)
                {
                    Console.WriteLine("На счету 0. Внесите средства");
                    break;
                }

                Console.Write("Сумма для снятия - ");
                var inputAmount = Console.ReadLine();

                decimal operationSum = 0;

                IncorrectFunds(inputAmount, out operationSum, operationSum, "Balance", clientAllData);

                clientAllData.Balance -= operationSum;
                Console.WriteLine($"Со счета снято {operationSum}, сумма на счету {clientAllData.Balance}");
                break;
            }

            return clientAllData.Balance;
        }

        private decimal PayCreditMoney(ClientAllData clientAllData)
        {
            while (true)
            {
                if (clientAllData.Credit == 0)
                {
                    Console.WriteLine("У вас нет кредитной задолженности");
                    break;
                }

                Console.Write($"Ваш кредит {clientAllData.Credit}\nСумма для погашения - ");
                var inputAmount = Console.ReadLine();

                decimal operationSum = 0;

                IncorrectInput(inputAmount, out operationSum);

                clientAllData.Credit -= operationSum;
                clientAllData.Balance -= operationSum;

                if (clientAllData.Credit <= 0)
                {
                    Console.WriteLine("Ваш кредит погашен");
                }
                else
                {
                    Console.WriteLine($"Внесено {operationSum}, остаток по кредиту {clientAllData.Credit}");
                }

                break;
            }

            return clientAllData.Credit;
        }

        private decimal GetCredit(ClientAllData clientAllData)
        {
            while (true)
            {
                Console.Write("Сумма кредита - ");
                var inputAmount = Console.ReadLine();

                decimal operationSum = 0;

                IncorrectInput(inputAmount, out operationSum);

                clientAllData.Credit += operationSum;
                Console.WriteLine($"Ваша кредитная задолженность составляет {clientAllData.Credit}");
                break;
            }

            return clientAllData.Credit;
        }

        private decimal PutDeposit(ClientAllData clientAllData)
        {
            while (true)
            {
                if(clientAllData.Deposit == 0)
                {
                    Console.WriteLine("У Вас нет депозита. Вы создаете депозитный счет под 15% годовых\n");
                }

                decimal yearPercent = 0.15M;
                decimal sumAfterYear = 0;

                Console.WriteLine($"На депозитном счету {clientAllData.Deposit}, на основном {clientAllData.Balance}");
                Console.Write("Сумма для депозита - ");

                var inputAmount = Console.ReadLine();

                decimal operationSum = 0;

                IncorrectFunds(inputAmount, out operationSum, operationSum, "Balance", clientAllData);

                clientAllData.Balance -= operationSum;
                clientAllData.Deposit += operationSum;
                sumAfterYear += clientAllData.Deposit * yearPercent + clientAllData.Deposit;

                Console.WriteLine($"На депозите {clientAllData.Deposit}\nЧерез год сумма будет составлять {sumAfterYear}");
                break;
            }

            return clientAllData.Deposit;
        }

        private decimal WithdrowDeposit(ClientAllData clientAllData)
        {
            while (true)
            {
                if (clientAllData.Deposit == 0)
                {
                    Console.WriteLine("На депозитном счету 0. Пополните счет");
                    break;
                }
                else
                {
                    Console.Write($"Доступно {clientAllData.Deposit}\nСумма для снятия - ");
                    var inputAmount = Console.ReadLine();

                    decimal operationSum = 0;

                    IncorrectFunds(inputAmount, out operationSum, operationSum, "Deposit", clientAllData);

                    Console.WriteLine("1. Сумму снять\n2. Перевести на баланс");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            clientAllData.Deposit -= operationSum;
                            Console.WriteLine($"Успешно снята сумма в {operationSum}\nОстаток на депозите {clientAllData.Deposit}");
                            break;
                        case "2":
                            clientAllData.Deposit -= operationSum;
                            clientAllData.Balance += operationSum;
                            Console.WriteLine($"Успешно переведено на баланс {operationSum}\nНа балансе {clientAllData.Balance}\nОстаток на депозите {clientAllData.Deposit}");
                            break;
                        default:
                            Console.WriteLine("Повторите ввод");
                            break;
                    }
                }
                break;
            }

            return clientAllData.Deposit;
        }

        private void IncorrectInput(string inputAmount, out decimal operationSum)
        {
            while (true)
            {
                if (decimal.TryParse(inputAmount, out operationSum) == false || operationSum <= 0)
                {
                    Console.WriteLine("Некорректный ввод");
                    continue;
                }
                break;
            }
        }

        private void IncorrectFunds(string inputAmount, out decimal operationSum, decimal inputSum, string accountType, ClientAllData clientAllData)
        {
            IncorrectInput(inputAmount, out operationSum);
            if (inputSum > clientAllData.Balance && accountType == "Balance")
            {
                Console.WriteLine("Введенная сумма превышает баланс на счету");
            }
            else if(inputSum > clientAllData.Deposit && accountType == "Deposit")
            {
                Console.WriteLine("Недостаточно средств на депозитном счету");
            }
            else if(inputSum > clientAllData.Balance && accountType == "Balance")
            {
                Console.WriteLine("Недостаточно средств на основном счету");
            }
        }
    }
}