using NvrOrganizer.Model;

namespace NvrOrganizer.UI.Wrapper
{
    public class NvrPhoneNumberWrapper : ModelWrapper<NvrPhoneNumber>
    {
        public NvrPhoneNumberWrapper(NvrPhoneNumber model) : base(model)
        {

        }

        public string Number
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
