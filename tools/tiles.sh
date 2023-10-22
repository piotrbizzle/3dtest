for i in {1..5}
do
    echo "Generating map tiles $i of 5"
    python3 ../../Scripts/tools/mapify.py $1$(($i-1))
done
