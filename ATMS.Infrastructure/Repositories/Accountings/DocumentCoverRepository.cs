
using System.Threading;
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using ATMS.Shared;
using ATMS.Shared.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ATMS.Infrastructure;

public class DocumentCoverRepository : IDocumentCoverRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<DocumentCoverRepository> _logger;

    public DocumentCoverRepository(IDbContextFactory<ApplicationDbContext> dbContext, IMapper mapper, ILogger<DocumentCoverRepository> logger)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
        this._logger = logger;
    }

    public async Task<Result<Guid>> AddAsync(DocumentCoverDto model, CancellationToken cancellationToken)
    {
        const string operation = nameof(AddAsync);

        _logger.LogDebug("Starting {Operation} for document number: {DocumentNumber}",
            operation, model.DocumentNumber);

        try
        {
            await using var context = await _dbContext.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

            // Check existence
            _logger.LogTrace("Checking if document {DocumentNumber} already exists", model.DocumentNumber);
            //var existDocCover = await context.DocumentCovers
            //    .FirstOrDefaultAsync(s => s.DocumentNumber == model.DocumentNumber, cancellationToken);

            //if (existDocCover is not null)
            //{
            //    _logger.LogWarning("Document {DocumentNumber} already exists with ID: {ExistingId}",
            //        model.DocumentNumber, existDocCover.Id);
            //    return Result<Guid>.Success(existDocCover.Id.Value);
            //}

            // Create entity
            var entity = _mapper.Map<DocumentCover>(model);
            entity.Id = DocumentCoverId.CreateNew();
            context.DocumentCovers.Add(entity);

            _logger.LogDebug("Saving new document {DocumentNumber} to database", model.DocumentNumber);

            var affected = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (affected > 0)
            {
                _logger.LogInformation("Successfully created document {DocumentNumber} with ID: {NewId}",
                    model.DocumentNumber, entity.Id);
                return Result<Guid>.Success(entity.Id.Value);
            }
            else
            {
                _logger.LogError("Failed to save document {DocumentNumber} - no rows affected",
                    model.DocumentNumber);
                return Result<Guid>.Failure("no rows affected");
            }
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx,
                "Database error in {Operation} for document {DocumentNumber}. Inner: {InnerMessage}",
                operation, model.DocumentNumber, dbEx.InnerException?.Message);
            throw;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex,
                "Unexpected error in {Operation} for document {DocumentNumber}",
                operation, model.DocumentNumber);
            throw;
        }
        finally
        {
            _logger.LogTrace("Completed {Operation} for document {DocumentNumber}",
                operation, model.DocumentNumber);
        }
    }

    public async Task<IReadOnlyList<DocumentCoverDto>> GetListAsync(CancellationToken cancellationToken)
    {
        const string operation = nameof(GetListAsync);

        _logger.LogDebug("Starting {Operation}", operation);

        try
        {
            await using var context = await _dbContext.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

            var list = await context.DocumentCovers.ToListAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<DocumentCoverDto>>(list);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx,
                "Database error in {Operation} for get document list. Inner: {InnerMessage}",
                operation, dbEx.InnerException?.Message);
            throw;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex,
                "Unexpected error in {Operation} for get document", operation);
            throw;
        }
        finally
        {
            _logger.LogTrace("Completed {Operation} for get document list",operation);
        }
    }
}
