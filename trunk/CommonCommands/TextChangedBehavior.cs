
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
namespace Common.Commands
{
    public class TextChangedBehavior : CommandBehaviorBase<TextBox>
    {
        public TextChangedBehavior(TextBox textBox)
            : base(textBox)
        {
            textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
        }

        void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                TextChangedArgs args = new TextChangedArgs()
                {
                    Sender = sender,
                    Text = textBox.Text
                };

                this.CommandParameter = args;
                base.ExecuteCommand();
            }
        }
    }
}
