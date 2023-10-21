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
                        

def main(file_name):
    try:
        os.mkdir("out") 
    except OSError as error:
        pass

    with Image.open(file_name) as img:
        screens_x = int(img.size[0] / 9)
        screens_y = int(img.size[1] / 7)
        for screen_x in range(screens_x):
            for screen_y in range(screens_y):
                screen_array = []
                for x in range(7):
                    row = []
                    for y in range(9):
                        pixel = img.getpixel((9 * screen_y + y, 7 * screen_x + x))
                        # -1 = empty, 0 = black, 1 = white
                        p_value = -1 if not pixel[0] else 0
                        row.extend(120 * [p_value])
                    for i in range(120):
                        screen_array.append(row)

                screen_img = Image.new(
                    "RGBA",
                    (9 * 120, 7 * 120),
                    color=(0,0,0,0),
                )
                draw = ImageDraw.Draw(screen_img)
                                
                for x in range(screen_img.size[1]):
                    for y in range(screen_img.size[0]):
                        p_value = screen_array[x][y]
                        if p_value == -1:
                            continue

                        # check if this is an outline
                        fill_value = 255 if _should_outline(screen_array, x, y) else 0                                                                
                        draw.point(
                            (y,x),
                            fill=(fill_value, fill_value, fill_value, 255),
                        )
                screen_img.save(
                    "out/" + str(screen_x) + "_" + str(screen_y) + "_" + file_name,
                    "PNG",
                )


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("need filenames")
    else:
        main(sys.argv[1])
