section .text

global PartOne
export PartOne
global PartTwo
export PartTwo

PartOne:
        xor     eax, eax
.loop:
        add     eax, [rdx]
        add     rdx, 4
        dec     ecx
        jnz     .loop
.end:
        ret

PartTwo:
        mov     eax, [rdx]
        dec     ecx
        jz     .end
.loop:
        add     rdx, 4
        mul     eax, [rdx]
        dec     ecx
        jnz     .loop
.end:
        ret
