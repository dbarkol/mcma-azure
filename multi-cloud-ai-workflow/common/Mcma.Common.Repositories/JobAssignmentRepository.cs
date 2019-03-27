
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Mcma.Common.Core.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Mcma.Common.Repositories
{
    public class JobAssignmentRepository
    {
        #region Data Members

        private readonly DocumentClient _documentClient;
        private readonly DocumentDbSettings _documentDbSettings;

        #endregion

        #region Constructors

        public JobAssignmentRepository(DocumentDbSettings settings)
        {
            // Save the document db settings into a private data member and 
            // instantiate an instance of the client.
            _documentDbSettings = settings;
            _documentClient = new DocumentClient(new Uri(_documentDbSettings.Endpoint),
                _documentDbSettings.AuthKey);
        }

        public JobAssignmentRepository() : this(DocumentDbSettings.GetDbSettings())
        {
        }

        #endregion

        #region Public Methods

        public async Task Initialize()
        {
            await CreateDatabaseIfNotExistsAsync();
            await CreateCollectionIfNotExistsAsync();

            var job1 = new JobAssignment() { Id = "1", Description = "Job1"};
            CreateJobDocumentIfNotExists(job1).Wait();

            var job2 = new JobAssignment() { Id = "2", Description = "Job2"};
            CreateJobDocumentIfNotExists(job2).Wait();
        }

        public List<JobAssignment> GetAllJobAssignments()
        {
            var results = _documentClient.CreateDocumentQuery<JobAssignment>(this.CollectionUri);
            return results.ToList();
        }

        public JobAssignment GetJobAssignmentById(string jobId)
        {
            var queryOptions = new FeedOptions { MaxItemCount = -1 };

            var query = _documentClient.CreateDocumentQuery<JobAssignment>(this.CollectionUri, queryOptions)
                .Where(p => p.Id.ToLower() == jobId.ToLower()).ToList();

            return query.FirstOrDefault();
        }

        public async Task CreateJobAssignment(JobAssignment job)
        {
            await _documentClient.CreateDocumentAsync(this.CollectionUri, job);
        }

        public async Task DeleteJobAssignment(string id)
        {
            await _documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_documentDbSettings.DatabaseId,
                _documentDbSettings.CollectionId, id));
        }

        public async Task UpdateJobAssignment(JobAssignment job)
        {
            // Check to see if the product exist
            var existingProduct = GetJobAssignmentById(job.Id);
            if (existingProduct != null)
            {
                await _documentClient.UpsertDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(_documentDbSettings.DatabaseId, _documentDbSettings.CollectionId),
                    job);
            }
        }

        #endregion

        #region Properties

        private Uri CollectionUri => UriFactory.CreateDocumentCollectionUri(_documentDbSettings.DatabaseId, _documentDbSettings.CollectionId);

        #endregion

        #region Private Methods

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                var databaseUri = UriFactory.CreateDatabaseUri(_documentDbSettings.DatabaseId);
                await _documentClient.ReadDatabaseAsync(databaseUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var database = new Database { Id = _documentDbSettings.DatabaseId };
                    await _documentClient.CreateDatabaseAsync(database);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                var collectionUri = UriFactory.CreateDocumentCollectionUri(_documentDbSettings.DatabaseId, _documentDbSettings.CollectionId);
                await _documentClient.ReadDocumentCollectionAsync(collectionUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var databaseUri = UriFactory.CreateDatabaseUri(_documentDbSettings.DatabaseId);
                    await _documentClient.CreateDocumentCollectionAsync(databaseUri,
                        new DocumentCollection { Id = _documentDbSettings.CollectionId },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateJobDocumentIfNotExists(JobAssignment job)
        {
            await _documentClient.UpsertDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(_documentDbSettings.DatabaseId, _documentDbSettings.CollectionId),
                job);
        }

        #endregion
    }
}
