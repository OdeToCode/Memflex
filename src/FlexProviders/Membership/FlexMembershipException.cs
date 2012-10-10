using System;
using System.Runtime.Serialization;

namespace FlexProviders.Membership
{
    [Serializable]
    public class FlexMembershipException : Exception
    {
        public FlexMembershipStatus StatusCode { get; set; }

        public FlexMembershipException()
        {
        }

        public FlexMembershipException(string message) : base(message)
        {
        }

        public FlexMembershipException(string message, Exception inner) : base(message, inner)
        {
        }

        public FlexMembershipException(FlexMembershipStatus statusCode)
        {
            this.StatusCode = statusCode;
        }

        protected FlexMembershipException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public enum FlexMembershipStatus
    {
        Success,
        InvalidUserName,
        InvalidPassword,
        InvalidQuestion,
        InvalidAnswer,
        InvalidEmail,
        DuplicateUserName,
        DuplicateEmail,
        UserRejected,
        InvalidProviderUserKey,
        DuplicateProviderUserKey,
        ProviderError,
    }
}