using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace nimble_life
{
    public partial class MainForm : Form
    {
        private Dictionary<string, float> latestGenes = new Dictionary<string, float>();
        private Dictionary<string, float> greatestGenes = new Dictionary<string, float>();
        private Board Board { get; set; }
        private Bitmap bm { get; set; }
        private int maxGenerations = 1;
        private int minGenerations = 2000;
        private Settings Settings = new Settings
        {
            Width = 50,
            Height = 50,
            //MaxAge = 100,
            Delay = 20, // //artificial delay in milliseconds. 0 for none
            AgeOfMaturity = 18,
            OneInThisIsHerby = 40,
            OneInThisIsRobot = 40,
        };

        public MainForm()
        {
            InitializeComponent();
            greatestGenes = new Dictionary<string, float>();
            greatestGenes[hg.EnergyToBaby.ToString()] = (float)0.16;
            greatestGenes[hg.MatingProbability.ToString()] = (float)0.7;
            greatestGenes[hg.MinFoodAvailableForBaby.ToString()] = (float)3.8;
            greatestGenes[hg.WorthMovingTo.ToString()] = (float)3.2;
            greatestGenes[hg.EnergyRequiredBeforeConsideringOffspring.ToString()] = 61;
            greatestGenes[hg.MaxAge.ToString()] = 343;
            greatestGenes[hg.MaxMutationFactor.ToString()] = (float)0.13;
            greatestGenes[hg.SpeciationDistance.ToString()] = (float)25;

            //See http://stackoverflow.com/questions/8046560/how-to-stop-flickering-c-sharp-winforms
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, this.splitContainerMain.Panel2, new object[] { true });
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            //create the world.
            this.Board = CreateBoard(this.Settings);

            //and force it to be painted.
            bm = RenderBoard();
            this.splitContainerMain.Panel2.Invalidate();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            if (bm == null) return;
            e.Graphics.DrawImage(bm, new Point(0, 0));
            
            RefreshBoard();
        }

        private void RefreshBoard()
        {
            maxGenerations = Math.Max(maxGenerations, Board.Generation);
            Board.Generation++;
            
            if (Board.Pieces== null || Board.Pieces.Count == 0 )
            {
                minGenerations = Math.Min(minGenerations, Board.Generation);
                this.Board = CreateBoard(this.Settings);
            }

            //HACK: Gone too long, so START AGAIN!
            //if (Board.Generation > 50000)
            //{
            //    this.Board = CreateBoard(this.Settings);
            //}

            if (Board.Tiles != null)
            {
                foreach(var t in Board.Tiles)
                {
                    t.Grass.TakeTurn(Board);
                }
            }

            if (Board.Pieces != null)
            {
                for (int i = Board.Pieces.Count - 1; i >= 0; i--)
                {
                    var p = Board.Pieces[i];
                    if (p is IAnimal)
                    {
                        var a = (p as IAnimal);
                        if (a.IsDead)
                        {
                            Board.Tiles[a.Location.X, a.Location.Y].Grass.Energy =
                                Math.Min(100,
                                    Board.Tiles[a.Location.X, a.Location.Y].Grass.Energy + a.Energy);

                            Board.Tiles[a.Location.X, a.Location.Y].Animal = null;
                            Board.Pieces.RemoveAt(i);
                        }
                    }
                }

                var babies = new List<IPiece>();
                foreach (var p in Board.Pieces)
                {
                    var baby = p.TakeTurn(Board);
                    if (baby != null) babies.Add(baby);
                    if (baby != null && baby is Herbivore)
                    {
                        var babyGenes = (baby as Herbivore).Genes;
                        latestGenes = Clone(babyGenes);
                    }
                }
                if (babies.Count > 0)
                {
                    foreach(var b in babies)
                    {
                        Board.Pieces.Add(b);
                    }
                }

                // check boundary type conditions
                foreach (var p in Board.Pieces)
                {
                    if (p is IAnimal)
                    {
                        var a = (p as IAnimal);

                        a.Age++;

                        if (a.Age > a.Genes["MaxAge"] || a.Energy <= 0)
                        {
                            a.IsDead = true;
                        }
                    }
                }
            }
            
            bm = RenderBoard();
            lblGeneration.Text = Board.Generation.ToString();
            lblStuff.Text = Board.Pieces.Count.ToString();
            lblMaxGenerations.Text = maxGenerations.ToString();
            lblMinGenerations.Text = minGenerations.ToString();
            this.splitContainerMain.Panel2.Invalidate();
            
            if (Settings.Delay > 0)
            {
                System.Threading.Thread.Sleep(Settings.Delay);
            }
        }

        private Dictionary<string, float> Clone(Dictionary<string, float> genes)
        {
            var result = new Dictionary<string, float>();

            foreach (var k in genes.Keys)
            {
                result[k] = genes[k];
            }

            return result;
        }

        private Bitmap RenderBoard()
        {
            if (Board == null) return null;

            var boardSizeInPixels = new Location
            {
                X = splitContainerMain.Panel2.Width,
                Y = splitContainerMain.Panel2.Height
            };

            var tileSizeInPixels = new Location
            {
                X = boardSizeInPixels.X / Board.Size.X,
                Y = boardSizeInPixels.Y / Board.Size.Y
            };

            var result = new Bitmap(boardSizeInPixels.X, boardSizeInPixels.Y);

            using (var g = Graphics.FromImage(result))
            {
                for (int x = 0; x < Board.Size.X; x++)
                {
                    for (int y = 0; y < Board.Size.Y; y++)
                    {
                        var tile = Board.Tiles[x, y];
                        g.FillRectangle(
                            new System.Drawing.SolidBrush(tile.Grass.Color),
                            x * tileSizeInPixels.X,
                            y * tileSizeInPixels.Y,
                            tileSizeInPixels.X,
                            tileSizeInPixels.Y);

                        var animal = tile.Animal;
                        if (animal != null)
                        {

                            var babyRatio = 0.1; //what size of an adult is a newborn baby? (e.g. 10% of adult size, then 0.1)
                            var ageOfMaturity = animal.AgeOfMaturity; //at what age are animals fullgrown?

                            var radius = new Location
                            {
                                X = (int)(((float)Math.Min(ageOfMaturity, animal.Age)*(1.0 - babyRatio) / ageOfMaturity + (babyRatio)) * (tileSizeInPixels.X/2.0)),
                                Y = (int)(((float)Math.Min(ageOfMaturity, animal.Age)*(1.0 - babyRatio) / ageOfMaturity + (babyRatio)) * (tileSizeInPixels.Y/2.0))
                            };

                            if (animal is Herbivore)
                            {
                                g.FillEllipse(
                                    new System.Drawing.SolidBrush(animal.Color),
                                    (float)((x + 0.5) * tileSizeInPixels.X) - radius.X,
                                    (float)((y + 0.5) * tileSizeInPixels.Y) - radius.Y,
                                    radius.X * 2,
                                    radius.Y * 2);
                            } else
                            {
                                g.FillRectangle(
                                    new System.Drawing.SolidBrush(animal.Color),
                                    (float)((x + 0.5) * tileSizeInPixels.X) - radius.X,
                                    (float)((y + 0.5) * tileSizeInPixels.Y) - radius.Y,
                                    radius.X * 2,
                                    radius.Y * 2);
                            }
                            if (animal.IsDead) // Draw thick Red X
                            {
                                g.DrawLine(new System.Drawing.Pen(System.Drawing.Brushes.Red, 3),
                                    x * tileSizeInPixels.X + (int)(tileSizeInPixels.X / 4.0),
                                    y * tileSizeInPixels.Y + (int)(tileSizeInPixels.Y / 4.0),
                                    x * tileSizeInPixels.X + (int)(tileSizeInPixels.X / 4.0) + (int)(tileSizeInPixels.X / 2.0),
                                    y * tileSizeInPixels.Y + (int)(tileSizeInPixels.Y / 4.0) + (int)(tileSizeInPixels.Y / 2.0));

                                g.DrawLine(new System.Drawing.Pen(System.Drawing.Brushes.Red, 3),
                                    x * tileSizeInPixels.X + (int)(tileSizeInPixels.X / 4.0) + (int)(tileSizeInPixels.X / 2.0),
                                    y * tileSizeInPixels.Y + (int)(tileSizeInPixels.Y / 4.0),
                                    x * tileSizeInPixels.X + (int)(tileSizeInPixels.X / 4.0),
                                    y * tileSizeInPixels.Y + (int)(tileSizeInPixels.Y / 4.0) + (int)(tileSizeInPixels.Y / 2.0));
                            }
                        }
                    }
                }
            }

            return result;
        }

        private Board CreateBoard(Settings settings)
        {
            var board = new Board
            {
                Size = new Location { X = settings.Width, Y = settings.Height },
                Tiles = new Tile[settings.Width, settings.Height],
                Pieces = new List<IPiece>(),
                //MaxAge = settings.MaxAge
            };

            int xpos = 0;
            int ypos = 0;
            for (int i = 0; i < settings.Width * settings.Height; i++)
            {
                var grass = new Grass
                {
                    Location = new Location { X = xpos, Y = ypos },
                    Energy = Rando.Next(50) //+ 50
                };

                board.Tiles[xpos, ypos] = new nimble_life.Tile
                {
                    Grass = grass,
                    Location = new Location { X = xpos, Y = ypos }
                };

                if (Rando.Next(settings.OneInThisIsHerby) ==1)
                {
                    var herby = new Herbivore()
                    {
                        Location = new nimble_life.Location { X = xpos, Y = ypos },
                        Energy = Rando.Next(75),
                        Species = "Sheep",
                        Color = System.Drawing.Color.Black,
                        Age = Rando.Next(30),
                        AgeOfMaturity = Settings.AgeOfMaturity,
                        Genes = new Dictionary<string, float>()
                    };
                    herby.Genes = Clone(greatestGenes);
                    board.Pieces.Add(herby);
                    board.Tiles[xpos, ypos].Animal = herby;
                }

                if (Rando.Next(settings.OneInThisIsRobot) == 1)
                {
                    var robby = new Robot()
                    {
                        Location = new nimble_life.Location { X = xpos, Y = ypos },
                        Energy = Rando.Next(75),
                        Species = "Umbrella",
                        Color = System.Drawing.Color.Black,
                        Age = Rando.Next(30),
                        AgeOfMaturity = Settings.AgeOfMaturity,
                        Genes = new Dictionary<string, float>()
                    };
                    robby.Genes = Clone(greatestGenes);
                    board.Pieces.Add(robby);
                    board.Tiles[xpos, ypos].Animal = robby;
                }




                xpos++;
                if (xpos >= settings.Width)
                {
                    xpos = 0;
                    ypos++;
                }
            }

            return board;
        }
    }
}
