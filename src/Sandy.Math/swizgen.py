def process(components, num_components, text, final_list):
    if num_components <= 0:
        return text;

    for comp in components:
        final_list.append(text + comp)
        process(components, num_components - 1, text + comp, final_list)

if __name__ == "__main__":
    components = list(input("Enter components you would like to generate swizzles for. (For example, RGBA) "))

    max_components = int(input("Enter the maximum number of components. "))

    struct_name = input("Enter a struct name.\nAvailable variables:\n\t\"{elem}\" - Element name\n\t\"{length}\" - The length of the element\n\t\"{params}\" - The elements in parameter form\n")

    output = input("Enter an output file name (leave blank for TTY output) (WARNING THIS WILL OVERWRITE!!!). ")

    final_list = []
    print("Processing... (this may take a while!)")
    process(components, max_components, "", final_list)

    print("Prettyfying... (this may also take a while!)")
    final_list = sorted(final_list, key=lambda x: len(x))

    # I hate the duplicate code but it was the only way I could make it "performant" (lol python moment)
    if output == '':
        for elem in final_list:
            split_elem = list(elem)
            length = len(split_elem)
            joj = ", ".join(split_elem) # oops, forgot to change this. guess it's joj now
            text = struct_name.replace("{elem}", elem).replace("{length}", str(length)).replace("{params}", joj).replace("\\n", "\n")
            print(text)
    else:
        prompt = input(f"Writing to output file \"{output}\"?? [Y/n] ")
        if prompt.lower() == 'y':
            with open(output, "w") as f:
                print("Writing... Please wait.")
                for elem in final_list:
                    split_elem = list(elem)
                    length = len(split_elem)
                    joj = ", ".join(split_elem) # oops, forgot to change this. guess it's joj now
                    text = struct_name.replace("{elem}", elem).replace("{length}", str(length)).replace("{params}", joj).replace("\\n", "\n")
                    f.write(text + "\n")
            print("All done.")
        else:
            print("Ok.")