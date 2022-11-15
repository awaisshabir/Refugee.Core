using System;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestService
    {
        private void ValidateShelterRequestOnAdd(ShelterRequest shelterRequest)
        {
            ValidateShelterRequestIsNotNull(shelterRequest);

            Validate(
                (Rule: IsInvalid(shelterRequest.Id), Parameter: nameof(ShelterRequest.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(shelterRequest.CreatedDate), Parameter: nameof(ShelterRequest.CreatedDate)),
                (Rule: IsInvalid(shelterRequest.CreatedByUserId), Parameter: nameof(ShelterRequest.CreatedByUserId)),
                (Rule: IsInvalid(shelterRequest.UpdatedDate), Parameter: nameof(ShelterRequest.UpdatedDate)),
                (Rule: IsInvalid(shelterRequest.UpdatedByUserId), Parameter: nameof(ShelterRequest.UpdatedByUserId)));
        }

        private static void ValidateShelterRequestIsNotNull(ShelterRequest shelterRequest)
        {
            if (shelterRequest is null)
            {
                throw new NullShelterRequestException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidShelterRequestException = new InvalidShelterRequestException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidShelterRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidShelterRequestException.ThrowIfContainsErrors();
        }
    }
}