using Castle.DynamicProxy;
using System.Transactions;
using UstaTakip.Core.Aspects.Interceptors;

namespace UstaTakip.Core.Aspects.Transaction
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TransactionScopeAspect : MethodInterception
    {
        public override void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptAsyncInternal(invocation);
        }

        public override void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptAsyncInternal(invocation);
        }

        private async Task InterceptAsyncInternal(IInvocation invocation)
        {
            using (var transactionScope = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await invocation.ProceedAsync(); // async metodu çalıştır
                    transactionScope.Complete(); // commit
                }
                catch
                {
                    throw; // rollback otomatik
                }
            }
        }
    }
}