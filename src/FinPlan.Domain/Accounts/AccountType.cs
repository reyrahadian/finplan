namespace FinPlan.Domain.Accounts
{
	public enum AccountType
	{
		//spending and savings
		Cash,
		CreditCard,
		Checking,
		Saving,
		OnlineAndDigitalWallet,
		Prepaid,
		GiftCard,

		//loans
		PersonalLoan,
		StudentLoan,
		AutoLoan,
		Mortgage,
		OtherLoan
	}
}