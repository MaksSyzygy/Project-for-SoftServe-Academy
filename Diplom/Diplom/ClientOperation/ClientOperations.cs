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
    class ClientOperations//Рассчет финансовых операций клиента
    {
        const string Balance = "Balance";
        const string Credit = "Credit";
        const string Deposit = "Deposit";
        const string DepositTransfer = "Deposit";

        public void ShowMyAccount(ClientAllData clientAllData)
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"\n{clientAllData.Intro()}\n");
            Console.WriteLine(new string('-', 50));
        }

        public void BalanceOperation(ClientAllData clientAllData)
        {
            Console.WriteLine("\n1. Начислить средства\n2. Снять средства\n");
            Console.Write("Ваш выбор - ");
            string balanceInput = Console.ReadLine();

            switch (balanceInput)
            {
                case "1":
                    WriteNewValueInDB(Balance, PutMoney(clientAllData), clientAllData);
                    break;
                case "2":
                    WriteNewValueInDB(Balance, WithdrawMoney(clientAllData), clientAllData);
                    break;
                default:
                    Console.WriteLine("Повторите ввод");
                    break;
            }
        }

        public void CreditOperation(ClientAllData clientAllData)
        {
            Console.WriteLine("\n1. Погасить кредит\n2. Взять кредит\n");
            Console.Write("Ваш выбор - ");
            string creditInput = Console.ReadLine();

            switch (creditInput)
            {
                case "1":
                    WriteNewValueInDB(Credit, PayCreditMoney(clientAllData), clientAllData);
                    break;
                case "2":
                    WriteNewValueInDB(Credit, GetCredit(clientAllData), clientAllData);
                    break;
                default:
                    Console.WriteLine("Повторите ввод");
                    break;
            }
        }

        public void DepositOperation(ClientAllData clientAllData)
        {
            Console.WriteLine("\n1. Положить деньги на депозит\n2. Снять деньги с депозита\n");
            Console.Write("Ваш выбор - ");
            string depositInput = Console.ReadLine();

            switch (depositInput)
            {
                case "1":
                    WriteNewValueInDB(Deposit, PutDeposit(clientAllData), clientAllData);
                    break;
                case "2":
                    WriteNewValueInDB(Deposit, WithdrowDeposit(clientAllData), clientAllData);
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

        private void OperationBalanceEffect(ClientAllData clientAllData)
        {
            WriteNewValueInDB(Balance, clientAllData.Balance, clientAllData);
        }

        private void CheckInput(string operationName, out decimal operationSum)
        {
            while (true)
            {
                Console.Write($"{operationName} - ");
                string inputAmount = Console.ReadLine();

                operationSum = 0;

                if (decimal.TryParse(inputAmount, out operationSum) == false || operationSum <= 0)
                {
                    Console.WriteLine("\nНекорректный ввод\n");
                    continue;
                }
                break;
            }
        }

        private void CheckIncorrectFunds(string operationName, out decimal operationSum,
            string accountType, ClientAllData clientAllData)
        {
            while (true)
            {
                CheckInput(operationName, out operationSum);
                if (operationSum > clientAllData.Balance && accountType == Balance)
                {
                    Console.WriteLine($"\nВведенная сумма превышает баланс на счету\nНа балансе доступно {clientAllData.Balance}\n");
                    continue;
                }
                else if (operationSum > clientAllData.Balance && accountType == Credit)
                {
                    Console.WriteLine($"\nНедостаточно средств для погашения кредита\nДля погашения доступно {clientAllData.Balance}\n");
                    continue;
                }
                else if (operationSum > clientAllData.Balance && accountType == Deposit)
                {
                    Console.WriteLine($"\nНедостаточно средств на основном счету\nНа балансе доступно {clientAllData.Deposit}\n");
                    continue;
                }
                else if (operationSum > clientAllData.Deposit && accountType == DepositTransfer)
                {
                    Console.WriteLine($"\nНедостаточно средств на депозитном счету\nНа депозите доступно {clientAllData.Deposit}\n");
                    continue;
                }
                break;
            }
        }

        private decimal PutMoney(ClientAllData clientAllData)
        {
            decimal operationSum = 0;

            while (true)
            {
                CheckInput("Сумма для зачисления", out operationSum);

                clientAllData.Balance += operationSum;
                Console.WriteLine($"\nНа счет внесено {operationSum}, сумма на счету {clientAllData.Balance}\n");
                break;
            }

            return clientAllData.Balance;
        }

        private decimal WithdrawMoney(ClientAllData clientAllData)
        {
            decimal operationSum = 0;

            while (true)
            {
                if(clientAllData.Balance == 0)
                {
                    Console.WriteLine("\nНа счету 0. Внесите средства\n");
                    break;
                }

                CheckIncorrectFunds("Сумма для снятия", out operationSum, Balance, clientAllData);

                clientAllData.Balance -= operationSum;
                Console.WriteLine($"\nСо счета снято {operationSum}, сумма на счету {clientAllData.Balance}\n");
                break;
            }

            return clientAllData.Balance;
        }

        private decimal PayCreditMoney(ClientAllData clientAllData)
        {
            decimal operationSum = 0;

            while (true)
            {
                if (clientAllData.Credit == 0)
                {
                    Console.WriteLine("\nУ вас нет кредитной задолженности\n");
                    break;
                }

                if(clientAllData.Balance == 0)
                {
                    Console.WriteLine("\nНа балансе 0, пополните счет\n");
                    break;
                }

                CheckIncorrectFunds($"Ваш кредит {clientAllData.Credit}\nДля погашения на балансе доступно " +
                    $"{clientAllData.Balance}\nСумма для погашения", 
                    out operationSum, Credit, clientAllData);

                clientAllData.Credit -= operationSum;
                clientAllData.Balance -= operationSum;

                OperationBalanceEffect(clientAllData);

                if (clientAllData.Credit <= 0)
                {
                    Console.WriteLine("\nВаш кредит погашен\n");
                }
                else
                {
                    Console.WriteLine($"\nВнесено {operationSum}, остаток по кредиту {clientAllData.Credit}\n");
                }

                break;
            }

            return clientAllData.Credit;
        }

        private decimal GetCredit(ClientAllData clientAllData)
        {
            decimal operationSum = 0;

            while (true)
            {
                CheckInput("Сумма кредита", out operationSum);

                clientAllData.Credit += operationSum;
                Console.WriteLine($"\nВаша кредитная задолженность составляет {clientAllData.Credit}\n");
                break;
            }

            return clientAllData.Credit;
        }

        private decimal PutDeposit(ClientAllData clientAllData)
        {
            decimal operationSum = 0;

            while (true)
            {
                if(clientAllData.Deposit == 0)
                {
                    Console.WriteLine("\nУ Вас нет депозита. Вы создаете депозитный счет под 15% годовых\n");
                }

                decimal yearPercent = 0.15M;
                decimal sumAfterYear = 0;

                Console.WriteLine($"\nНа депозитном счету {clientAllData.Deposit}, на основном {clientAllData.Balance}\n");

                CheckIncorrectFunds("Сумма для депозита", out operationSum, Deposit, clientAllData);

                clientAllData.Deposit += operationSum;
                clientAllData.Balance -= operationSum;

                OperationBalanceEffect(clientAllData);

                sumAfterYear += clientAllData.Deposit * yearPercent + clientAllData.Deposit;

                Console.WriteLine($"\nНа депозите {clientAllData.Deposit}\nЧерез год сумма будет составлять {sumAfterYear}\n");
                break;
            }

            return clientAllData.Deposit;
        }

        private decimal WithdrowDeposit(ClientAllData clientAllData)
        {
            decimal operationSum = 0;

            while (true)
            {
                if (clientAllData.Deposit == 0)
                {
                    Console.WriteLine("\nНа депозитном счету 0. Пополните счет\n");
                    break;
                }
                else
                {
                    CheckIncorrectFunds($"Доступно {clientAllData.Deposit}\nСумма для снятия", out operationSum,
                        DepositTransfer, clientAllData);

                    Console.WriteLine("\n1. Сумму снять\n2. Перевести на баланс\n");
                    Console.Write("Ваш выбор - ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            clientAllData.Deposit -= operationSum;
                            Console.WriteLine($"\nУспешно снята сумма в {operationSum}\nОстаток на депозите {clientAllData.Deposit}\n");
                            break;
                        case "2":
                            clientAllData.Deposit -= operationSum;
                            clientAllData.Balance += operationSum;

                            OperationBalanceEffect(clientAllData);

                            Console.WriteLine($"\nУспешно переведено на баланс {operationSum}\n" +
                                $"На балансе {clientAllData.Balance}\nОстаток на депозите {clientAllData.Deposit}\n");
                            break;
                        default:
                            Console.WriteLine("\nПовторите ввод\n");
                            break;
                    }
                }
                break;
            }

            return clientAllData.Deposit;
        }
    }
}