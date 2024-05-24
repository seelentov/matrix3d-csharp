namespace BlazorApp1.Models.Matrix
{
    public class Matrix3D<T>
    {
        private T[,,] _map;
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Length { get; private set; }

        public T this[int i, int j, int m]
        {
            get { return _map[i, j, m]; }
            set { _map[i, j, m] = value; }
        }

        public Matrix3D(int width, int height, int length)
        {
            _map = new T[height, width, length];
            Height = height;
            Width = width;
            Length = length;
        }
        public override string ToString()
        {
            string result = "";

            result += "[\n";

            for (int i = 0; i < Height; i++)
            {
                result += $" Y:{i}\n [\n";
                for (int j = 0; j < Width; j++)
                {
                    result += "     ";
                    result += $"X:{j}[";
                    for (int m = 0; m < Length; m++)
                    {

                        result += _map[i, j, m];
                        if (m < Length - 1)
                        {
                            result += ", ";
                        }

                    }
                    result += "]\n";
                }
                result += " ]\n";

            }
            result += "]";

            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    for (int m = 0; m < Length; m++)
                    {
                        yield return _map[i, j, m];
                    }
                }
            }
        }

        public bool IsCellExist(Point point)
        {
            return (point.X >= 0 && point.X < Width) && (point.Y >= 0 && point.Y < Height) && (point.Z >= 0 && point.Z < Length);
        }
        public void Fill(T data)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    for (int m = 0; m < Length; m++)
                    {
                        _map[i, j, m] = data;
                    }
                }
            }
        }

        public T Get(Point point)
        {
            if (!IsCellExist(point))
            {
                throw new ArgumentOutOfRangeException($"Точки {point.ToString()} не существует");
            }

            return _map[point.Y, point.X, point.Z];
        }

        public void Set(T data, Point point)
        {
            if (!IsCellExist(point))
            {
                throw new ArgumentOutOfRangeException($"Точки {point.ToString()} не существует");
            }

            _map[point.Y, point.X, point.Z] = data;
        }

        public bool Move(Point point, Direction direction, Func<T,bool> checkRule = null)
        {
            T data = Get(point);
        
            Point newPoint = point.getPointOnDirection(direction);
            if (!IsCellExist(newPoint))
            {
                return false;
            }

            if (checkRule != null)
            {
                if (!checkRule(Get(newPoint)))
                {
                    return false;
                }
            }


            Set(default(T), point);
            Set(data, newPoint);
            return true;
        }

    }

    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

        public Point getPointOnDirection(Direction direction)
        {
            int newX = X;
            int newY = Y;
            int newZ = Z;

            switch (direction)
            {
                case Direction.up:
                    newY--; break;
                case Direction.down:
                    newY++; break;
                case Direction.left:
                    newX--; break;
                case Direction.right:
                    newX++; break;
                case Direction.back:
                    newZ--; break;
                case Direction.forward:
                    newZ++; break;
            }

            return new Point(newX, newY, newZ);
        }
        public void Move(Direction direction)
        {
            this = getPointOnDirection(direction);
        }
    }

    public enum Direction
    {
        up, down, left, right, forward, back
    }
}
