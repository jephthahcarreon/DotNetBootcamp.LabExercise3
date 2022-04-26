using System;

namespace CSharp.LabExercise3
{
    class UserInputValidator
    {
        Account account;

        public UserInputValidator()
        {
        }
        public UserInputValidator(Account account)
        {
            this.account = account;
        }

        public int ValidateUserChoiceInput()
        {
            int userChoiceAsInt;
            while (true)
            {
                Console.Write("\n\tPlease select transaction: ");
                string userChoiceAsString = Console.ReadLine();

                try
                {
                    userChoiceAsInt = Convert.ToInt32(userChoiceAsString);
                    if (userChoiceAsInt >= 1 && userChoiceAsInt <= 4)
                    {

                        return userChoiceAsInt;
                    }
                    else
                    {
                        Console.WriteLine("\tInvalid input. Please enter a number between 1 and 4.");
                        continue;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\tInvalid input. Please enter a number.");
                    continue;
                }
            }
        }

        public decimal ValidateUserWithdrawAmountInput()
        {

            decimal withdrawAmountInput;
            while (true)
            {
                Console.Write("\n\tPlease enter withdrawal amount: ");
                string withdrawAmountInputAsString = Console.ReadLine();

                try
                {
                    withdrawAmountInput = Convert.ToDecimal(withdrawAmountInputAsString);
                    if (withdrawAmountInput >= 0)
                    {
                        goto SecondValidation;
                    }
                    else
                    {
                        Console.WriteLine("\tInvalid amount. Amount should be a positive integer value.");
                        continue;
                    }
                    SecondValidation:
                    if (withdrawAmountInput % 100 == 0)
                    {
                        goto ThirdValidation;
                    }
                    else
                    {
                        Console.WriteLine("\tAmount must be divisible by 100.");
                        continue;
                    }
                    ThirdValidation:
                    if (withdrawAmountInput <= account.AccountBalance)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\tInsufficient Account Balance.");
                        continue;
                    }
                }
                catch
                {
                    throw new ArgumentException("Invalid Withdrawal Amount.");
                }
            }

            return withdrawAmountInput;
        }

        public decimal ValidateUserDepositAmountInput()
        {
            decimal depositAmountInput;
            while (true)
            {
                Console.Write("\n\tPlease enter deposit amount: ");
                string depositAmountInputAsString = Console.ReadLine();

                try
                {
                    depositAmountInput = Convert.ToDecimal(depositAmountInputAsString);
                    if (depositAmountInput >= 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\tInvalid Deposit Amount.");
                        continue;
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException("Invalid Deposit Amount.");
                }
            }
            return depositAmountInput;
        }
    }

    class UserInterfaceRenderer
    {
        public void RenderMainScreenOptions()
        {
            Console.WriteLine("**********Welcome to ATM Service**********\n");
            Console.WriteLine("1. Check Balance\n");
            Console.WriteLine("2. Withdraw Cash\n");
            Console.WriteLine("3. Deposit Cash\n");
            Console.WriteLine("4. Quit\n");
            Console.WriteLine("******************************************\n");
        }
    }

    class CashDepositService
    {
        Account account;
        public CashDepositService(Account account)
        {
            this.account = account;
        }
        public decimal Deposit(decimal amount)
        {
            try
            {
                if(amount>=0)
                {
                    account.AccountBalance += amount;
                }
                return account.AccountBalance;

            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid Deposit Amount.");
            }
        }
    }

    class CashWithdrawalService
    {
        Account account;
        public CashWithdrawalService (Account account)
        {
            this.account=account;
        }
        public decimal Withdraw(decimal amount)
        {
            try
            {
                if (amount > 0)
                {
                    goto SecondValidation;
                }
            }
            catch
            {
                throw new ArgumentException("Invalid amount. Amount should be a positive integer value.");
            }
            SecondValidation:
            try
            {
                if (amount % 100 == 0)
                {
                    goto ThirdValidation;
                }
            }
            catch
            {
                throw new ArgumentException("Amount must be divisible by 100.");
            }
            ThirdValidation:
            try
            {
                if (amount < account.AccountBalance)
                {
                    account.AccountBalance -= amount;
                }
                return account.AccountBalance;
            }
            catch
            {
                throw new ArgumentException("Insufficient Account Balance.");
            }
        }
    }

    class CheckBalanceService
    {
        Account account;
        public CheckBalanceService (Account account)
        {
            this.account = account;
        }
        public void CheckAccountBalance()
        {
            string dateTime = DateTime.Now.ToString("h:mm:ss tt");
            string dateToday = DateTime.Today.ToString("dd/MM/yyyy");
            Console.WriteLine("\n\tAccount Balance: {0}", account.AccountBalance);
            Console.WriteLine("\tDate: {0}", dateToday);
            Console.WriteLine("\tTime: {0}", dateTime);
        }
    }
    
    class Account
    {
        decimal _accountBalance;
        public decimal AccountBalance
        {
            get { return _accountBalance; }
            set
            {
                try
                {
                    if (value > 0)
                    {
                        _accountBalance = value;
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException("Invalid Account Balance.");
                }
            }
        }
        public Account()
        {

        }
        public Account(decimal accountBalance)
        {
            _accountBalance = accountBalance;
        }
    }

    class AutomatedTellerMachineApplication
    {
        public void RunApplication()
        {
            Account account = new Account();
            UserInterfaceRenderer userInterfaceRenderer = new UserInterfaceRenderer();
            UserInputValidator userInputValidator = new UserInputValidator(account);
            CheckBalanceService checkBalanceService = new CheckBalanceService(account);
            CashDepositService cashDepositService = new CashDepositService(account);
            CashWithdrawalService cashWithdrawalService = new CashWithdrawalService(account);


            while (true)
            {
                userInterfaceRenderer.RenderMainScreenOptions();
                int userChoiceAsInt = userInputValidator.ValidateUserChoiceInput();

                switch (userChoiceAsInt)
                {

                    // Check Balance
                    case 1:
                        Console.Clear();
                        userInterfaceRenderer.RenderMainScreenOptions();
                        Console.WriteLine("Check Account Balance");
                        checkBalanceService.CheckAccountBalance();
                        break;

                    // Withdraw Cash
                    case 2:
                        Console.Clear();
                        userInterfaceRenderer.RenderMainScreenOptions();
                        Console.WriteLine("Withdraw Cash");
                        decimal withdrawAmount = userInputValidator.ValidateUserWithdrawAmountInput();
                        cashWithdrawalService.Withdraw(withdrawAmount);
                        Console.WriteLine("\tWithdraw transaction successful.");
                        checkBalanceService.CheckAccountBalance();
                        break;

                    // Deposit Cash
                    case 3:
                        Console.Clear();
                        userInterfaceRenderer.RenderMainScreenOptions();
                        Console.WriteLine("Deposit Cash");
                        decimal depositAmount = userInputValidator.ValidateUserDepositAmountInput();
                        cashDepositService.Deposit(depositAmount);
                        Console.WriteLine("\tDeposit transaction successful.");
                        checkBalanceService.CheckAccountBalance();
                        break;

                    // Exit
                    case 4:
                        Console.Clear();
                        goto ExitApp;
                }

                while (true)
                {
                    //prompts user to exit or continue
                    Console.Write("\n\nWould you like to make another transaction?\n(y/n): ");
                    string userChoiceInput = Console.ReadLine();
                    Console.WriteLine("");

                    //catch errors from invalid input
                    try
                    {
                        char userChoiceInputChar = char.ToLower(Convert.ToChar(userChoiceInput));

                        switch (userChoiceInputChar)
                        {
                            case 'y':
                                Console.Clear();
                                break;
                            case 'n':
                                Console.Clear();
                                goto ExitApp;
                            default:
                                Console.WriteLine("Invalid input.");
                                continue;
                        }
                        break;
                    }

                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input.");
                        continue;
                    }
                }
            }

            ExitApp:

            Console.WriteLine("Thank you\nTransaction completed");
            Console.WriteLine("Press any key to close this application . . .");
            Console.ReadLine();
            Console.Clear();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AutomatedTellerMachineApplication automatedTellerMachineApplication = new AutomatedTellerMachineApplication();
            automatedTellerMachineApplication.RunApplication();
        }
    }
}