for i in {1..5}
do
    echo "Moving $i of 5"
    mv $1$i.png $1$(($i-1)).png
done
