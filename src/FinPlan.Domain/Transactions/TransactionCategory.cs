namespace FinPlan.Domain.Transactions
{
	public class TransactionCategory
	{
		public TransactionCategory()
		{
		}

		public TransactionCategory(string name)
		{
			Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }
	}
}