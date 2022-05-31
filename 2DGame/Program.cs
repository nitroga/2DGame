using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

Raylib.InitWindow(Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), "2D Game");
Raylib.SetTargetFPS(60);
Raylib.ToggleFullscreen();

Random rnd = new Random();
Texture2D firstSpaceShip = Raylib.LoadTexture(@"firstspaceship.png");
Texture2D stone1 = Raylib.LoadTexture(@"stone1.png");
Texture2D stone2 = Raylib.LoadTexture(@"stone2.png");
Texture2D stone3 = Raylib.LoadTexture(@"stone3.png");
Texture2D spaceBase = Raylib.LoadTexture(@"spacebase.png");
int rotation = 0;
int stones = 0;
int space = 0;
int j = 0;
int playerHealth = Raylib.GetScreenWidth()/7;
bool inv = false;
List<Vector2> stonePos = new List<Vector2>();
List<Rectangle> collisions = new List<Rectangle>();
List<Texture2D> stoneTextures = new List<Texture2D>();
for (int i = 0; i < 15; i++)
{
    stoneTextures.Add(stone1);
    stoneTextures.Add(stone2);
    stoneTextures.Add(stone3);
}
Rectangle healthBorder = new Rectangle(80, 30, Raylib.GetScreenWidth() / 7 + 10, 30);
Rectangle health = new Rectangle(85, 35, playerHealth, 20);
Rectangle playerCollisionBox = new Rectangle(Raylib.GetScreenWidth()/2, Raylib.GetScreenHeight()/2, 32, 32);

Vector2 position = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
Vector2 laserStart = new Vector2(Raylib.GetScreenWidth() / 2 + 16, Raylib.GetScreenHeight() / 2 + 16);
Vector2 basePos = new Vector2(900, 575);

for (int i = 0; i < 45; i++)
{
    stonePos.Add(new Vector2(rnd.Next(-3000, 3000), rnd.Next(-3000, 3000)));
}
double xspeed = 0;
double yspeed = 0;

while (!Raylib.WindowShouldClose())
{
    space = stones;
    collisions.Clear();
    yspeed*= 0.95;
    xspeed*= 0.95;

    (xspeed, yspeed) = movement(xspeed, yspeed);

    if (Raylib.IsKeyPressed(KeyboardKey.KEY_E)) {
        inv = !inv;
    }

    /* if (position.X + 100 > basePos.X && position.X < basePos.X + 200 && position.Y > basePos.Y - 100 && position.Y - 150 < basePos.Y) {
        Raylib.DrawText("Press 'F' to park", Raylib.GetScreenWidth()/2-70, 200, 28, Color.WHITE);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_F)) {
            basePos = new Vector2(900, 575);
        }
    } */

    checkCollisions();

    float fxspeed = (float)xspeed;
    float fyspeed = (float)yspeed;

    basePos += new Vector2(fxspeed, fyspeed);

    for (int i = 0; i < stonePos.Count; i++)
    {
        stonePos[i]+= new Vector2(fxspeed, fyspeed);
    }
    Vector2 mousePos = Raylib.GetMousePosition();
    if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
    {
        if (mousePos.X < 760 || mousePos.X > 1160 || mousePos.Y < 340 || mousePos.Y > 740 ) {
            Raylib.DrawText("Laser too far out", Raylib.GetScreenWidth()/2-100, 50, 30, Color.WHITE);
        }
        else {
            for (int i = 0; i < 45; i++)
            {
                collisions.Add(new Rectangle(stonePos[i].X, stonePos[i].Y, 50, 50));
                if (Raylib.CheckCollisionPointRec(mousePos, collisions[i])) {
                    j++;
                    if (j == 30) {
                        if(stones > 99 == false) {
                            stones++;
                        }
                    j = 0;
                    }
                }
            }
            Raylib.DrawLineEx(laserStart, mousePos, 5, Color.SKYBLUE);
        }
    }
    string stoneText = stones.ToString();
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.BLACK);
    Raylib.DrawFPS(0, 0);
    if (playerHealth >= 0) {
    Raylib.DrawRectangleRec(healthBorder, Color.WHITE);
    Raylib.DrawRectangleRec(health, Color.GREEN);
    for (int i = 0; i < stonePos.Count; i++)
    {
        Raylib.DrawTextureEx(stoneTextures[i], stonePos[i], 0, 1f, Color.WHITE);
        Raylib.DrawRectangle((int)stonePos[i].X - 10, (int)stonePos[i].Y - 15, 70, 10, Color.GREEN);
    }
    Raylib.DrawTextureEx(firstSpaceShip, position, rotation, 0.9f, Color.WHITE);
    Raylib.DrawRectangle((int)position.X, (int)position.Y, 18, 18, Color.BLANK);
    Raylib.DrawText("Health", 180, 8, 22, Color.WHITE);
    Raylib.DrawTextureEx(spaceBase, basePos, 0, 1.25f, Color.WHITE);
    if (inv == true) {
            Raylib.DrawRectangle(Raylib.GetScreenWidth()/2-250, 60, 500, 300, Color.WHITE);
            Raylib.DrawRectangle(Raylib.GetScreenWidth()/2-240, 70, 480, 280, Color.BLACK);
            Raylib.DrawText("Inventory", Raylib.GetScreenWidth()/2-70, 22, 32, Color.WHITE);
            Raylib.DrawText("Stones", Raylib.GetScreenWidth()/2-225, 75, 24, Color.WHITE);
            Raylib.DrawRectangle(Raylib.GetScreenWidth()/2-225, 100, 85, 60, Color.WHITE);
            Raylib.DrawRectangle(Raylib.GetScreenWidth()/2-220, 105, 75, 50, Color.BLACK);
            Raylib.DrawText(stoneText, Raylib.GetScreenWidth()/2-212, 115, 35, Color.WHITE);
        }
        if (inv == true && space > 99 == false) {
            Raylib.DrawText("Inventory Space: " + space.ToString() + " out of 100", Raylib.GetScreenWidth()/2-160, 320, 23, Color.WHITE);
        }
        if (space > 99) {
            Raylib.DrawText("Inventory Full!", Raylib.GetScreenWidth()/2-70, 320, 23, Color.WHITE);
        }
    }
    else {
        Raylib.DrawText("You died", Raylib.GetScreenWidth()/2-100, Raylib.GetScreenHeight()/2, 50, Color.RED);
    }
    Raylib.EndDrawing();
}

void checkCollisions() {
    collisions.Add(new Rectangle(basePos.X, basePos.Y, 150, 150));
    for (int i = 0; i < 45; i++)
    {
        collisions.Add(new Rectangle(stonePos[i].X, stonePos[i].Y, 50, 50));
        if (Raylib.CheckCollisionRecs(playerCollisionBox, collisions[i])) {
            for (int j = 0; j < 45; j++)
            {
                xspeed *= 0.92;
                yspeed *= 0.92;
                }
                double immunityTime = 0;
                if (Raylib.GetTime() - immunityTime > 2) {
                    playerHealth--;
                    immunityTime = Raylib.GetTime();
                }
                health = new Rectangle(85, 35, playerHealth, 20);
            }
        }
    }

    static (double, double) movement(double xspeed, double yspeed) {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
    {
        yspeed = 1.5;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
    {
        yspeed = -1.5;
    }

    if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
    {
        xspeed = -1.5;
    }

    if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
    {   
        xspeed = 1.5;
    }
    return (xspeed, yspeed);
    }