using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelectionCamouflage
{
    internal static class Utils
    {
        private static Random random = new Random();

        private static (float, float, float, float) canvasVertices = (float.MinValue, float.MinValue, float.MinValue, float.MinValue);

        private static (float minX, float minY, float maxX, float maxY) GetBoundingBox(Polygon2D polygon = null)
        {
            if (canvasVertices != (float.MinValue, float.MinValue, float.MinValue, float.MinValue))
                return canvasVertices;

            if (polygon.Polygon.Length == 0)
                return (0, 0, 0, 0);

            float minX = polygon.Polygon[0].x;
            float maxX = polygon.Polygon[0].x;
            float minY = polygon.Polygon[0].y;
            float maxY = polygon.Polygon[0].y;

            foreach (Vector2 point in polygon.Polygon)
            {
                if (point.x < minX) minX = point.x;
                if (point.x > maxX) maxX = point.x;
                if (point.y < minY) minY = point.y;
                if (point.y > maxY) maxY = point.y;
            }

            // Add some margins
            minX += 20f;
            minY += 5f;
            maxX -= 20f;
            maxY -= 5;

            canvasVertices = (minX, minY, maxX, maxY);
            return canvasVertices;
        }

        public static void Move(Area2D sprite)
        {
            // Keep trying to move until we move in a valid way (such that we end up in the desired box)
            while (true)
            {
                // Generate a random angle in degrees
                float randomAngle = (float)random.NextDouble() * 360.0f;

                // Convert the angle to radians
                float angleRad = Mathf.Deg2Rad(randomAngle);

                // Calculate the direction vector
                Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

                // The amount moved per tick in the original NetLogo was 5
                // This is an amount meant to be similar
                int movementConstant = 50;

                // Move the sprite
                sprite.Position += direction * movementConstant;

                if (!Utils.IsInBoundingBox(sprite.Position))
                    sprite.Position -= direction * movementConstant; // move the opposite direction instead to stay in the box
                else
                    return;
            }
        }

        public static bool IsInBoundingBox(Vector2 location)
        {
            float minX = canvasVertices.Item1;
            float minY = canvasVertices.Item2;
            float maxX = canvasVertices.Item3;
            float maxY = canvasVertices.Item4;

            return location.x >= minX && location.x <= maxX && location.y >= minY && location.y <= maxY;
        }

        public static Vector2 GetRandomPointInPolygon(Polygon2D polygon)
        {
            if (polygon.Polygon.Length < 3)
                return Vector2.Zero;

            (float minX, float minY, float maxX, float maxY) = GetBoundingBox(polygon);

            Vector2 randomPoint;
            Random random = new Random();
            while (true)
            {
                float x = (float)random.NextDouble() * (maxX - minX) + minX;
                float y = (float)random.NextDouble() * (maxY - minY) + minY;
                randomPoint = new Vector2(x, y);

                if (Geometry.IsPointInPolygon(randomPoint, polygon.Polygon))
                    break;
            }

            return randomPoint;
        }

        private static ShaderMaterial _colorInverter = null;
        public static ShaderMaterial ColorInverter
        {
            get
            {
                if (_colorInverter != null)
                    return _colorInverter;
                Shader shader = new Shader();
                shader.Code = @"
                    shader_type canvas_item;

                    void fragment() {
                        // Get the texture color at the current fragment (pixel) location
                        vec4 color = texture(TEXTURE, UV);
                
                        // Invert the color
                        color.rgb = vec3(1.0 - color.rgb);
                
                        // Output the color
                        COLOR = color;
                    }
                ";

                _colorInverter = new ShaderMaterial();
                _colorInverter.Shader = shader;
                return _colorInverter;
            }
        }
    }
}
