using ImGuiNET;
using System;
using System.Numerics;
using WondrousTailsList.DataStructures;

namespace WondrousTailsList
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class PluginUI : IDisposable
    {
        private Configuration configuration;

        private readonly WondrousTailsBook wondrousTailsBook = new();

        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        // passing in the image here just for simplicity
        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
        }

        public void Draw()
        {
            // This is our only draw handler attached to UIBuilder, so it needs to be
            // able to draw any windows we might have open.
            // Each method checks its own visibility/state to ensure it only draws when
            // it actually makes sense.
            // There are other ways to do this, but it is generally best to keep the number of
            // draw delegates as low as possible.

            DrawMainWindow();
        }

        public void DrawMainWindow()
        {
            if (!Visible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(350, 330), new Vector2(float.MaxValue, float.MaxValue));
            if (ImGui.Begin("Wondrous Tails List", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {

                var CopyText = "**Wondrous Tails List**" + Environment.NewLine;
                CopyText = CopyText + "Due: " + wondrousTailsBook.GetDeadline() + Environment.NewLine;
                CopyText = CopyText + "--------------------------------------" + Environment.NewLine;

                ImGui.Text($"Has a book: {wondrousTailsBook.PlayerHasBook()}");
                ImGui.Text($"New Book Available: {wondrousTailsBook.NewBookAvailable()}");
                ImGui.Text($"Deadline: {wondrousTailsBook.GetDeadline()}");

                ImGui.Spacing();

                ImGui.Text($"Stickers: {wondrousTailsBook.GetNumStickers()}/9");
                ImGui.Indent(25);
                ImGui.Text($"2nd Chances: {wondrousTailsBook.GetNumSecondChance()}");
                ImGui.Unindent(25);

                ImGui.Spacing();
                Vector4 Grey = new(0.6f, 0.6f, 0.6f, 1.0f);
                ImGui.Separator();
                ImGui.Spacing();

                ImGui.Text($"Current Book Tasks");
                ImGui.Indent(25);

                Vector4 SoftGreen = new(0.2f, 0.8f, 0.2f, 1.0f);
                foreach (dynamic task in wondrousTailsBook.GetAllTaskData())
                {
                    if (task.TaskState.ToString() == "Unavailable")
                    {
                        ImGui.TextColored(Grey, $"- {task.Taskname}");
                        CopyText = CopyText + " - ~~" + task.Taskname + "~~" + Environment.NewLine;
                    } else
                    {
                        ImGui.Text($"- {task.Taskname}");
                        CopyText = CopyText + " - " + task.Taskname + Environment.NewLine;
                    }
                }

                ImGui.Spacing();
                ImGui.Spacing();

                if (ImGui.Button("Copy List"))
                {
                    ImGui.SetClipboardText(CopyText);
                }
            }
            ImGui.End();
        }
    }
}
