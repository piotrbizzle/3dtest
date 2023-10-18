import os
import sys

from PIL import Image, ImageDraw
import numpy

def main(depth, file_name):
    try:
        os.mkdir("out") 
    except OSError as error:
        pass

    offset_bases = range(5) if depth == None else [int(depth)]
    
    with Image.open(file_name) as img:
        for offset_base in offset_bases:
            offset = 10 * (offset_base + 1)
            l_array, r_array = [], []            
            for x in range(img.size[1]):
                l_array.append([])            
                r_array.append([])
                r_array[x] += [-1] * (2 * offset)
                for y in range(img.size[0]):
                    pixel = img.getpixel((y,x))
                    # -1 = empty, 0 = black, >0 = white/grey
                    p_value = -1 if not pixel[1] else pixel[0]
                    l_array[x].append(p_value)
                    r_array[x].append(p_value)
                l_array[x] += [-1] * (2 * offset)

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
                    if l_p_value > 0 and r_p_value > 0:
                        fill_value = int((l_p_value + r_p_value) / 2)
                        m_draw.point(
                            (x, y),
                            fill=(fill_value, fill_value, fill_value, 255),
                        )
                    elif l_p_value > 0:
                        l_draw.point(
                            (x, y),
                            fill=(l_p_value, l_p_value, l_p_value, 255),
                        )
                    elif r_p_value > 0:
                        r_draw.point(
                            (x, y),
                            fill=(r_p_value, r_p_value, r_p_value, 255),
                        )
                    elif r_p_value == 0:
                        r_draw.point((x, y), fill=(0, 0, 0, 255))
                    elif l_p_value == 0:
                        l_draw.point((x, y), fill=(0, 0, 0, 255))
            l_img.save("out/" + str(offset_base) + "l_"+ file_name, "PNG")
            m_img.save("out/" + str(offset_base) + "m_" + file_name, "PNG")
            r_img.save("out/" + str(offset_base) + "r_" + file_name, "PNG")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("need filenames")
    elif len(sys.argv) == 2:
        main(None, sys.argv[1])
    else:
        main(sys.argv[1], sys.argv[2])
