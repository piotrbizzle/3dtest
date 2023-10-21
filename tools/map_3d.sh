for i in {0..4}
do
    echo "3Difying $(($i+1)) of 5"
    python3 ../../../Scripts/tools/3dify.py $i $1$i.png
done
