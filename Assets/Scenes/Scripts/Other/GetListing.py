from os import walk
import os
from shutil import copyfileobj

dir_path = os.path.dirname(os.path.realpath(__file__))

from os import walk
filesNames = next(walk(dir_path), (None, None, []))[2]

for fileName in filesNames:
    if(os.path.basename(__file__) == fileName):
        continue
    if(fileName.__contains__(".meta") == True):
        continue

    with open(dir_path + "\\" + "result.txt", 'a') as output:
        output.write("\n--[Начало {0}]--\n".format(fileName))

    with open(dir_path + "\\" + "result.txt", 'ab') as output, open(dir_path + "\\" + fileName, 'rb') as input:
        copyfileobj(input, output)
        
    with open(dir_path + "\\" + "result.txt", 'a') as output:
        output.write("\n--[Конец {0}]--\n".format(fileName))
