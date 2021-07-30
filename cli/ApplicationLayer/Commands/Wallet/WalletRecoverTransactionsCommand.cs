using System;
using System.Threading.Tasks;
using BAMWallet.HD;
using Kurukuru;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using BAMWallet.Extensions;
using BAMWallet.Helper;
namespace CLi.ApplicationLayer.Commands.Wallet
{
    [CommandDescriptor("recover", "Recover wallet transactions")]
    public class WalletRecoverTransactionsCommand : Command
    {
        private readonly IWalletService _walletService;

        private Spinner _spinner;

        public WalletRecoverTransactionsCommand(IServiceProvider serviceProvider) : base(typeof(WalletRecoverTransactionsCommand).GetAttributeValue((CommandDescriptorAttribute attr) => attr.Name),
            typeof(WalletRecoverTransactionsCommand).GetAttributeValue((CommandDescriptorAttribute attr) => attr.Description), serviceProvider.GetService<IConsole>())
        {
            _walletService = serviceProvider.GetService<IWalletService>();
        }

        public override async Task Execute()
        {
            this.Login();
            using var KeepLoginState = new RAIIGuard(Command.FreezeTimer, Command.UnfreezeTimer);
            await Spinner.StartAsync("Recovering transactions ...", async spinner =>
            {
                _spinner = spinner;
                var session = ActiveSession;

                await _walletService.RecoverTransactions(session, 0);

                return Task.CompletedTask;
            }, Patterns.Pong);
        }
    }
}