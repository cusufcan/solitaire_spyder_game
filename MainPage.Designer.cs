namespace solitaire_spyder_game {
    partial class MainPage {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            SuspendLayout();
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1008, 661);
            ForeColor = SystemColors.ControlText;
            MinimumSize = new Size(1024, 700);
            Name = "MainPage";
            ShowIcon = false;
            Text = "Solitaire Spyder";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}
