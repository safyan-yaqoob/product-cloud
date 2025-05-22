using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductCloud.SharedKernal.CQRS;
using ProductService.Database.Entities;

namespace ProductService.Features.DeletePlan
{
	public sealed class DeletePlanCommandHandler(ProductDbContext context) : ICommandHandler<DeletePlanCommand, bool>
	{
		public async Task<bool> Handle(DeletePlanCommand command, CancellationToken cancellationToken = default)
		{
			var plan = await context.Set<Plan>().FirstOrDefaultAsync(p => p.Id == command.PlanId, cancellationToken);

			if (plan == null) return false;

			context.Set<Plan>().Remove(plan);

			await context.SaveChangesAsync(cancellationToken);

			return true;
		}
	}
}
