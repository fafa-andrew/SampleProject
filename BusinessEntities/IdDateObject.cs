using System;

namespace BusinessEntities
{
    public class IdDateObject : IdObject
    {
        private DateTime _createdOn;
        private DateTime? _modifiedOn;

        public DateTime CreatedOn
        {
            get => _createdOn;
            set => _createdOn = DateTime.UtcNow;
        }

        public DateTime? ModifiedOn
        {
            get => _modifiedOn;
            set => _modifiedOn = value;
        }

        public void SetModifiedDate(DateTime? modifiedOn)
        {
            if (modifiedOn is null) { _modifiedOn = null; return; }
            if (modifiedOn < _createdOn)
                throw new ArgumentOutOfRangeException(nameof(modifiedOn), "Modified date must be on or after the created date.");
            
            _modifiedOn = modifiedOn.Value;
        }
    }
}
