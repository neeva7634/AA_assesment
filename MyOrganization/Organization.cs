using System;
using System.Text;
using System.Collections.Generic;

namespace MyOrganization
{
    internal abstract class Organization
    {
        private Position root;
        private Dictionary<string, Position> positionMap;

        public Organization()
        {
            root = CreateOrganization();
            positionMap = new Dictionary<string, Position>();
            BuildPositionMap(root);
        }

        protected abstract Position CreateOrganization();

        private int nextEmployeeIdentifier = 1;

        private int GetNextEmployeeIdentifier()
        {
            return nextEmployeeIdentifier++;
        }

        public Position? Hire(Name person, string title)
        {
            if (positionMap.TryGetValue(title, out Position position))
            {
                if (!position.IsFilled())
                {
                    position.SetEmployee(new Employee(GetNextEmployeeIdentifier(), person));

                    return position;
                }
                else
                {
                    Console.WriteLine($"Position {title} is already filled.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"Position {title} does not exist in the organization.");
                return null;
            }
        }

        override public string ToString()
        {
            return PrintOrganization(root, "");
        }

        private void BuildPositionMap(Position position)
        {
            positionMap[position.GetTitle()] = position;
            foreach (Position directReport in position.GetDirectReports())
            {
                BuildPositionMap(directReport);
            }
        }

        private string PrintOrganization(Position pos, string prefix)
        {
            StringBuilder sb = new StringBuilder(prefix + "+-" + pos.ToString() + "\n");
            foreach (Position p in pos.GetDirectReports())
            {
                sb.Append(PrintOrganization(p, prefix + "  "));
            }
            return sb.ToString();
        }
    }
}
