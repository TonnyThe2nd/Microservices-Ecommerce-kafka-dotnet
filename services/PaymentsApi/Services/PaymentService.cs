public class PaymentService
{
    public PaymentStatus ProcessPayment(int orderId, decimal amount)
    {
        // simulação simples de regra de negócio

        if (amount <= 0)
            return PaymentStatus.Failed;

        // simula chance de falha (exemplo realista)
        var random = new Random();

        var approved = random.Next(0, 10) > 2; // 80% sucesso

        return approved ? PaymentStatus.Success : PaymentStatus.Failed;
    }
}