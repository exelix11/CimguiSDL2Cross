// SDL.h provides the SDL_Main() symbol and must be kept
#include "SDL.h"
  
extern void cross_example_entrypoint();

int main(int argc, char *argv[])
{
  cross_example_entrypoint();
  return 0;
}