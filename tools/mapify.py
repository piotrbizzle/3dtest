import os
import sys

from PIL import Image, ImageDraw
import numpy

def _should_outline(screen_array, x, y):
    for i in (-2, 0, 2):
        for j in (-2, 0, 2):
            test_x = x + i
            test_y = y + j
            
            # outline screen edges always
            if test_x < 0 or test_x >= len(screen_array):
                return True
            if test_y < 0 or test_y >= len(screen_array[0]):
                return True
            if screen_array[test_x][test_y] == -1:
                return True
    return False
                        

def main(file_prefix):
    try:
        os.mkdir("out") 
    except OSError as error:
        pass

    with Image.open(file_prefix[:-1] + "map" + file_prefix[-1] + ".png") as map_img, Image.open(file_prefix[:-1] + "collisions" + file_prefix[-1] + ".png") as collisions_img:
        screens_x = int(map_img.size[0] / 9)
        screens_y = int(map_img.size[1] / 7)
        for screen_x in range(screens_y):
            for screen_y in range(screens_x):
                screen_array = []

                # scan map image
                for x in range(7):
                    row = []
                    for y in range(9):
                        pixel = map_img.getpixel((9 * screen_y + y, 7 * screen_x + x))
                        # -1 = empty, 0 = black, 1 = white
                        p_value = -1 if not pixel[0] else 0
                        row.extend(120 * [p_value])

                    for i in range(120):
                        screen_array.append(row)

                # build fullsize image
                screen_img = Image.new(
                    "RGBA",
                    (9 * 120, 7 * 120),
                    color=(0,0,0,0),
                )
                draw = ImageDraw.Draw(screen_img)
                                
                for x in range(screen_img.size[1]):
                    for y in range(screen_img.size[0]):
                        p_value = screen_array[x][y]

                        # skip drawing unless this is an outline
                        if p_value == -1:
                            continue
                        if not _should_outline(screen_array, x, y):
                            continue
                        draw.point(
                            (y,x),
                            fill=(255, 255, 255, 255),
                        )
                screen_img.save(
                    "out/" + str(screen_x) + "_" + str(screen_y) + "_" + file_prefix[:-1] + "map" + file_prefix[-1] + ".png",
                    "PNG",
                )

                # generate collision code
                collisions_string = str(screen_x) + "_" + str(screen_y) + "_" + file_prefix + "\n\n    {{"
                for x in range(7):
                    for y in range(9):
                        pixel = collisions_img.getpixel((9 * screen_y + y, 7 * screen_x + x))
                        if pixel[0]:
                            collisions_string += "1,"
                        else:
                            collisions_string += "0,"
                    # end of the line
                    collisions_string = collisions_string[:-1]
                    collisions_string += "},\n     {"
                collisions_string = collisions_string[:-9] + "}},"
                print(collisions_string)

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("need fileprefix")
    else:
        main(sys.argv[1])
