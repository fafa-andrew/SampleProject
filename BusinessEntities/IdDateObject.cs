using System;

namespace BusinessEntities
{
    public class IdDateObject : IdObject
    {
        private DateTime _createdOn;
        private DateTime? _modifiedOn;

        public DateTime CreatedOn => _createdOn;
        public DateTime? ModifiedOn => _modifiedOn;

        public IdDateObject() => _createdOn = DateTime.UtcNow;

        public void SetModifiedDate(DateTime? modifiedOn)
        {
            if (modifiedOn is null) { _modifiedOn = null; return; }
            if (modifiedOn < _createdOn)
                throw new ArgumentOutOfRangeException(nameof(modifiedOn), "Modified date must be on or after the created date.");
            
            _modifiedOn = modifiedOn.Value;
        }
    }
}
