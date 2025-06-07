using System;

namespace Etheral.Interfaces
{
    public interface IFeedback
    {
        public event Action<FeedbackType> OnHitFeedback;

        void HapticFeedback(FeedbackType feedbackType);

    }
}
