using SC_M4.Forms;
using SC_M4.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4
{
    public partial class Main
    {
        private CancellationTokenSource cts_auto = new CancellationTokenSource();
        private Task task_auto;
        public void InitializeAutoTest()
        {

        }

        private void StartAutoTest()
        {
            if (taskProcess != null && taskProcess.Status == TaskStatus.Running)
            {
                return;
            }

            cts_auto?.Dispose();
            cts_auto = new CancellationTokenSource();
            task_auto = Task.Run(() => DoWorkAutoTest(cts_auto.Token), cts_auto.Token);
        }
        private void StopAutoTest()
        {

        }

        private List<ActionIO> actionIO;
        private void DoWorkAutoTest(CancellationToken token)
        {
            if (string.IsNullOrEmpty(txtModel.Text))
            {
                if (MessageBox.Show("Please input model name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    return;
                }

                return;
            }
            Console.WriteLine("Start Process...");
            IsChangeSelectedMode = false;
            // 
            actionIO = ActionIO.Get();

            var model = Models.Get(txtModel.Text);
            // 1. Check model
            if (model == null)
            {
                if (MessageBox.Show("Model not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    return;
                }
            }

            // Get Item
            var items = Items.GetItemsByModelId(model.id);
            if (items == null || items.Count == 0)
            {
                if (MessageBox.Show("Item not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    return;
                }
            }

            // For each item
            foreach(var item in items){
                List<Actions> actions = Actions.GetListByItemId(item.id);
                foreach(var action in actions)
                {
                    switch ((Utilities.TypeAction)action.type)
                    {
                        case Utilities.TypeAction.Auto:
                            //Console.WriteLine($"Action name {action.name}");
                            ProcessTypeAuto(action, token);
                            break;
                        case Utilities.TypeAction.Manual:
                            //Console.WriteLine($"Action name {action.name}");
                            ProcessTypeManual(action, token);
                            break;
                            case Utilities.TypeAction.Servo:
                            Console.WriteLine($"Action name {action.name}");
                            break;
                        case Utilities.TypeAction.Image:
                            Console.WriteLine($"Action name {action.name}");
                            break;
                    }
                    Thread.Sleep(action.delay);
                }
            }
        }

        private void ProcessTypeAuto(Actions action,CancellationToken token)
        {
            // Set parameter
            ActionIO iO = actionIO.FirstOrDefault(x => x.id == action.action_io_id);
            if (iO == null)
            {
                Console.WriteLine($"Action IO not found");
                return;
            }
            int pin = iO.pin;
            byte value = ((byte)pin);

            templateData["Command_io"][2] = 0X49;
            templateData["Command_io"][3] = 0x50;
            templateData["Command_io"][4] = value;
            templateData["Command_io"][6] = 0x01;
            // Send parameter
            SerialCommand(templateData["Command_io"]);
            Thread.Sleep(action.auto_delay);
            // Set parameter
            templateData["Command_io"][6] = 0x00;
            // Send parameter
            SerialCommand(templateData["Command_io"]);
        }


        private void ProcessTypeManual(Actions action, CancellationToken token)
        {
            // Set parameter
            ActionIO iO = actionIO.FirstOrDefault(x => x.id == action.action_io_id);
            if (iO == null)
            {
                Console.WriteLine($"Action IO not found");
                return;
            }
            int pin = iO.pin;
            byte value = ((byte)pin);
            templateData["Command_io"][2] = 0X49;
            templateData["Command_io"][3] = 0x50;
            templateData["Command_io"][4] = value;
            templateData["Command_io"][6] = action.state == 1 ? (byte)0x01 : (byte)0x00; ;
            // Send parameter
            SerialCommand(templateData["Command_io"]);
        }

        private void ProcessTypeServo(Actions action, CancellationToken token)
        {

        }
        private void ProcessTypeImage(Actions action, CancellationToken token)
        {

        }

    }
}


