using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace nimble_life
{
    public static class Rando
    {
        private static Random r = new Random(Guid.NewGuid().GetHashCode());
        public static int Next(int limit)
        {
            return r.Next(limit);
        }

        public static double Next()
        {
            return r.NextDouble();
        }

        public static float Either(float num1, float num2)
        {
            return r.Next(1) == 0 ? num1 : num2;
        }

        // shuffle the members of a list (in-place)
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                //swap n with k.
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    public class Board
    {
        public Location Size { get; set; }
        public List<IPiece> Pieces { get; set; }
        public Tile[,] Tiles { get; set; }
        //public int MaxAge { get; set; }
        public int Generation { get; set; }

        internal List<Tile> GetNeighbors(int x, int y)
        {
            var result = new List<Tile>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var offsetX = i - 1;
                    var offsetY = j - 1;
                    var newX = x + offsetX;
                    var newY = y + offsetY;
                    if (newX < 0) newX = Size.X - 1;
                    if (newY < 0) newY = Size.Y - 1;
                    if (newX >= Size.X) newX = 0;
                    if (newY >= Size.Y) newY = 0;

                    if (offsetX != 0 || offsetY != 0) // ignore current tile...
                    {
                        result.Add(Tiles[newX, newY]);
                    }
                }
            }

            Rando.Shuffle(result);

            return result;
        }
    }
    public class Tile
    {
        public Grass Grass { get; set; }
        public IAnimal Animal { get; set; }
        public Location Location { get; set; }
    }

    public class Grass : IPiece
    {
        public string[] Actions { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }

        public IPiece TakeTurn(Board board)
        {
            // Grass quickly grows... 
            if (Energy < 99) Energy = Energy + (float)0.15;
            if (Energy > 99) Energy = 99;

            var green = Math.Min((int)(Energy * (256.0 / 100.0)), 255);

            this.Color = Color.FromArgb(0, green, 0);
            return null;
        }
    }

    enum hg //herbivore genes
    {
        WorthMovingTo,
        MatingProbability,
        MinFoodAvailableForBaby,
        EnergyToBaby,
        EnergyRequiredBeforeConsideringOffspring,
        MaxAge,
        MaxMutationFactor,
        SpeciationDistance
    }

    public class Herbivore : IPiece, IAnimal
    {
        public string[] Actions { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public List<IAnimal> MatingOffers { get; set; }
        public string Species { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public int AgeOfMaturity { get; set; }
        public Dictionary<string, float> Genes { get; set; }

        public void ChooseAction(List<IPiece> neighbours)
        {
            throw new NotImplementedException();
        }

        public void Eat(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void OfferToMate(IAnimal animal)
        {
            throw new NotImplementedException();
        }

        public IPiece TakeTurn(Board board)
        {
            if (this.IsDead) return null;

            IPiece result = null;

            var moveDone = false;
            var currentTile = board.Tiles[this.Location.X, this.Location.Y];

            var neighbors = board.GetNeighbors(this.Location.X, this.Location.Y);

            var bestNeighbor = currentTile;

            foreach (var t in neighbors)
            {
                if (t.Animal == null && t.Grass.Energy > bestNeighbor.Grass.Energy)
                {
                    bestNeighbor = t;
                }
            }

            if (!moveDone && bestNeighbor != currentTile &&
                (bestNeighbor.Grass.Energy > this.Genes[hg.WorthMovingTo.ToString()]))
            {
                // Move to it!
                currentTile.Animal = null;
                bestNeighbor.Animal = this;
                this.Location.X = bestNeighbor.Location.X;
                this.Location.Y = bestNeighbor.Location.Y;
                this.Energy -= 5; //TAKES energy to move...
                moveDone = true;
            }
            else
            {
                // Takes energy to stand still!
                this.Energy -= 1;
            }

            if (!moveDone && currentTile.Grass.Energy > 15 && this.Energy < 100)
            {
                // Chew some.
                currentTile.Grass.Energy -= 15;
                this.Energy += 10; // Note inefficiency
            }

            this.Energy = Math.Min(100, this.Energy);
            
            // Consider trying to mate...
            if (Rando.Next() < this.Genes[hg.MatingProbability.ToString()]
                            && this.Age >= this.AgeOfMaturity)
            {
                var hottestNeighbor = this as IAnimal;
                foreach (var t in neighbors)
                {
                    if (t.Animal != null
                        && t.Animal.Species == this.Species // picky
                        && t.Animal.IsDead == false   // picky!
                        && t.Animal.Age > t.Animal.AgeOfMaturity // picky
                        && t.Animal.Energy >= hottestNeighbor.Energy) //picky
                    {
                        hottestNeighbor = t.Animal;
                    }
                }

                if (hottestNeighbor != this) // picky
                {
                    if (hottestNeighbor.MatingOffers == null) hottestNeighbor.MatingOffers = new List<IAnimal>();
                    // swipe right
                    hottestNeighbor.MatingOffers.Add(this);
                }
            }

            if (this.MatingOffers != null && this.Energy > this.Genes[hg.EnergyRequiredBeforeConsideringOffspring.ToString()])
            {
                var bestOffer = this.MatingOffers
                    .Where(p =>
                            p.IsDead == false
                        && p.Species == this.Species
                        && p.Age > p.AgeOfMaturity)
                        .OrderByDescending(p => p.Energy).FirstOrDefault();

                if (bestOffer != null)
                {
                    // Find an empty neighboring square to plonk a baby on...
                    bestNeighbor = null;

                    foreach (var t in neighbors)
                    {
                        if (t.Animal == null
                            && (bestNeighbor == null || t.Grass.Energy > bestNeighbor.Grass.Energy)
                            && t.Grass.Energy > this.Genes[hg.MinFoodAvailableForBaby.ToString()]) // what kind of a world are we bringing this child into...
                        {
                            bestNeighbor = t;
                        }
                    }

                    if (bestNeighbor != null)
                    {
                        var baby = new Herbivore
                        {
                            Species = this.Species,
                            Energy = (int)(this.Energy * this.Genes[hg.EnergyToBaby.ToString()]),
                            Age = 0,
                            AgeOfMaturity = this.AgeOfMaturity,
                            Genes = new Dictionary<string, float>()
                        };


                        var MaxMutationFactor = this.Genes[hg.MaxMutationFactor.ToString()];

                        foreach (var d in this.Genes.Keys)
                        {
                            var mutationFactor = (float)1 + (float)((Rando.Next() * MaxMutationFactor) - (MaxMutationFactor / 2));

                            // Cross over and mutation
                            baby.Genes[d] = Rando.Either(bestOffer.Genes[d], this.Genes[d]) * mutationFactor;
                        }

                        // Having babies really takes something out of you.
                        this.Energy -= baby.Energy;

                        baby.Location = new Location()
                        {
                            X = bestNeighbor.Location.X,
                            Y = bestNeighbor.Location.Y
                        };
                        bestNeighbor.Animal = baby;
                        result = baby;

                        // Get rid of all mating offers.
                        this.MatingOffers = null;
                    }
                    else
                    {
                        Debug.WriteLine("No empty file found.");
                    }
                }
            }

            // shade of gray depends on energy levels
            var gray = Math.Max(0, Math.Min((int)(Energy * (256.0 / 100.0)), 255));

            this.Color = Color.FromArgb(gray, gray, gray);
            return result;
        }

        private float GeneDistance(IAnimal animal1, IAnimal animal2)
        {
            var totalDiff = (float)0;
            foreach(var g in animal1.Genes.Keys)
            {
                totalDiff += Math.Abs(animal1.Genes[g] - animal2.Genes[g]);
            }
            return totalDiff;
        }
    }

    public class Omnivore : IPiece, IAnimal
    {
        public string[] Actions { get; set; }
        public Dictionary<string, float> Genes { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public List<IAnimal> MatingOffers { get; set; }
        public string Species { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public int AgeOfMaturity { get; set; }

        public void ChooseAction(List<IPiece> neighbours)
        {
            throw new NotImplementedException();
        }

        public void Eat(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void OfferToMate(IAnimal animal)
        {
            throw new NotImplementedException();
        }

        public IPiece TakeTurn(Board board)
        {
            return null;
        }
    }

    public class Carnivore : IPiece, IAnimal
    {
        public string[] Actions { get; set; }
        public Dictionary<string, float> Genes { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public List<IAnimal> MatingOffers { get; set; }
        public string Species { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public int AgeOfMaturity { get; set; }

        public void ChooseAction(List<IPiece> neighbours)
        {
            throw new NotImplementedException();
        }

        public void Eat(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void OfferToMate(IAnimal animal)
        {
            throw new NotImplementedException();
        }

        public IPiece TakeTurn(Board board)
        {
            return null;
        }
    }

    public interface IAnimal : IPiece
    {
        void ChooseAction(List<IPiece> neighbours);
        Dictionary<string, float> Genes { get; set; }
        void Eat(IPiece piece);
        void Move();
        bool IsDead { get; set; }
        int Age { get; set; }
        int AgeOfMaturity { get; set; }
        void OfferToMate(IAnimal animal);
        List<IAnimal> MatingOffers { get; set; }
        string Species { get; set; }
    }

    public interface IPiece
    {
        Location Location { get; set; }
        float Energy { get; set; }
        IPiece TakeTurn(Board board);
        string[] Actions { get; set; }
        Color Color { get; set; }
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
