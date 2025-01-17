﻿using TrafficCourts.Staff.Service.OpenAPIs.OracleDataApi.v1_0;

namespace TrafficCourts.Staff.Service.Services;

public interface IDisputeService
{
    /// <summary>Returns all the existing disputes from the database.</summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A collection of Dispute objects</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task<ICollection<Dispute>> GetAllDisputesAsync(CancellationToken cancellationToken);

    /// <summary>Saves new dispute in the oracle database.</summary>
    /// <param name="dispute"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The identifier of the saved Dispute record.</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task<Guid> SaveDisputeAsync(Dispute dispute, CancellationToken cancellationToken);

    /// <summary>Returns a specific dispute from the database.</summary>
    /// <param name="id">Unique identifier of a Dispute record.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>OK</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task<Dispute> GetDisputeAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Updates the properties of a particular Dispute record based on the given values.</summary>
    /// <param name="id">Unique identifier of a Dispute record to modify.</param>
    /// <param name="dispute">A modified version of the Dispute record to save.</param>    
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The modified Dispute record.</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task<Dispute> UpdateDisputeAsync(Guid id, Dispute dispute, System.Threading.CancellationToken cancellationToken);

    /// <summary>Updates the status of a particular Dispute record to VALIDATED.</summary>
    /// <param name="id">Unique identifier of a Dispute record to validate.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task ValidateDisputeAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Updates the status of a particular Dispute record to CANCELLED.</summary>
    /// <param name="id">Unique identifier of a Dispute record to cancel.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task CancelDisputeAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Updates the status of a particular Dispute record to REJECTED.</summary>
    /// <param name="id">Unique identifier of a Dispute record to cancel.</param>
    /// <param name="rejectedReason">The reason or note (max 256 characters) for the rejection.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task RejectDisputeAsync(Guid id, string rejectedReason, CancellationToken cancellationToken);

    /// <summary>Submits a Dispute, setting it's status to PROCESSING.</summary>
    /// <param name="id">Unique identifier of a Dispute record to submit.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task SubmitDisputeAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>An endpoint to delete a specific dispute in the database.</summary>
    /// <param name="id">Unique identifier of a Dispute record to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task DeleteDisputeAsync(Guid id, CancellationToken cancellationToken);
}
