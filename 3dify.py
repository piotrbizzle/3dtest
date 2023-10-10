import os
import sys

from PIL import Image, ImageDraw
import numpy

def main(file_name):
    try:
        os.mkdir("out") 
    except OSError as error:
        pass

    with Image.open(file_name) as img:
        for offset_base in range(5):
            offset = 10 * (offset_base + 1)
            l_array, r_array = [], []            
            for x in range(img.size[0]):
                l_array.append([])            
                r_array.append([])
                r_array[x] += [0] * (2 * offset)
                for y in range(img.size[1]):
                    pixel = img.getpixel((y,x))
                    # 0 = empty, 1 = black, 2 = white
                    p_value = 0 if not pixel[1] else (1 if not pixel[0] else 2)
                    l_array[x].append(p_value)
                    r_array[x].append(p_value)
                l_array[x] += [0] * (2 * offset)

            l_img = Image.new("RGBA", (len(l_array[0]), len(l_array)), color=(0, 0, 0, 0))
            l_draw = ImageDraw.Draw(l_img)
            m_img = Image.new("RGBA", (len(l_array[0]), len(l_array)), color=(0, 0, 0, 0))
            m_draw = ImageDraw.Draw(m_img)
            r_img = Image.new("RGBA", (len(l_array[0]), len(l_array)), color=(0, 0, 0, 0))
            r_draw = ImageDraw.Draw(r_img)

            for x in range(l_img.size[0]):
                for y in range(l_img.size[1]):
                    l_p_value = l_array[y][x]
                    r_p_value = r_array[y][x]
                    if l_p_value == 2 and r_p_value ==2:
                        m_draw.point((x, y), fill=(255, 255, 255, 255))
                    elif l_p_value == 2:
                        l_draw.point((x, y), fill=(255, 255, 255, 255))
                    elif r_p_value == 2:
                        r_draw.point((x, y), fill=(255, 255, 255, 255))
                    elif r_p_value == 1:
                        r_draw.point((x, y), fill=(0, 0, 0, 255))
                    elif l_p_value == 1:
                        l_draw.point((x, y), fill=(0, 0, 0, 255))
            l_img.save("out/" + str(offset_base) + "l_"+ file_name, "PNG")
            m_img.save("out/" + str(offset_base) + "m_" + file_name, "PNG")
            r_img.save("out/" + str(offset_base) + "r_" + file_name, "PNG")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("need filename")
    else:
        main(sys.argv[1])
