for i in {1..5}
do
    echo "Moving $i of 5"
    mv $1$i.png $1$(($i-1)).png

    echo "Generating map tiles $i of 5"
    python3 ../../Scripts/tools/mapify.py $1$(($i-1)).png
done

	    
